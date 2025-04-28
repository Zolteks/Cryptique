using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UI_DialogueManager : Singleton<UI_DialogueManager>
{
    #region Serializable Class
    [System.Serializable]
    public class DialogueCharacter
    {
        public string sNameEN;
        public string sNameFR;
        public string sNameDisplay;
        public Sprite iTalkingPortrait;
        public Sprite iListeningPortrait;
        public bool bTalkOnRightSide;
        public bool bNoPortrait;
    }

    [System.Serializable]
    public class DialogueLine
    {
        public DialogueCharacter cCharacter;
        [TextArea(3, 10)]
        public string sLine;
    }

    [System.Serializable]
    public class Dialogue
    {
        public List<DialogueLine> lDialogueLines = new List<DialogueLine>();
    }

    #endregion Serializable Class

    public Image iLeftCharacterPortrait;
    public Image iRightCharacterPortrait;
    public TextMeshProUGUI tNameText;
    public TextMeshProUGUI tDialogueDisplay;

    public GameObject dialoguePanel;

    private LanguageManager m_languageManager;
    private PC_PlayerController m_playerController;

    private Queue<DialogueLine> qLines;
    public bool bisDialogueActive = false;
    public float defaultTextSpeed = 0.02f;
    public float maxTextSpeed = 0.0001f;
    private float m_currentTextSpeed;
    private bool bIsTypingText = false;

    [SerializeField] private GameObject UI_Play;

    void Awake()
    {
        m_languageManager = LanguageManager.Instance;
        if (m_languageManager == null)
            Debug.LogError("LanguageManager is null");
        m_playerController = PC_PlayerController.Instance;
        if(m_playerController == null)
            Debug.LogError("PlayerController is null");
        m_currentTextSpeed = defaultTextSpeed;
        HideDialogueUI(0);
    }

    public void ShowDialogueUI()
    {
        UI_Play.SetActive(false);

        m_playerController.DisableInput();
        dialoguePanel.SetActive(true);
    }

    public void HideDialogueUI(float time)
    {
        StartCoroutine(CoroutineDeactivateAfterAnimation(time));
        UI_Play.SetActive(true);
    }

    IEnumerator CoroutineDeactivateAfterAnimation(float time)
    {
        yield return new WaitForSeconds(time); // Animation Length
        dialoguePanel.SetActive(false);
        bisDialogueActive = false;
    }

    public void StartDialogue(Dialogue c_Dialogue)
    {
        bisDialogueActive = true;
        
        if (qLines == null)
            qLines = new Queue<DialogueLine>();
        else
            qLines.Clear();
        
        foreach (DialogueLine cLine in c_Dialogue.lDialogueLines)
        {
            LanguageCode currentLanguage = m_languageManager.GetCurrentLanguage();
            qLines.Enqueue(cLine);
        }
        
        DisplayNextLine();
    }

    public void ButtonDisplayNextLine()
    {
        DisplayNextLine();
    }
    
    public void DisplayNextLine()
    {
        if (bIsTypingText == true)
        {
            m_currentTextSpeed = maxTextSpeed;
            return;
        }
        if (qLines.Count == 0)
        {
            EndDialogue();
            m_playerController.EnableInput();
            return;
        }
        
        DialogueLine cCurrentLine = qLines.Dequeue();
        tNameText.text = cCurrentLine.cCharacter.sNameDisplay;
        
        // Masquer tous les portraits si bNoPortrait est true
        if (cCurrentLine.cCharacter.bNoPortrait)
        {
            iRightCharacterPortrait.gameObject.SetActive(false);
            iLeftCharacterPortrait.gameObject.SetActive(false);
        }
        else
        {
            Image talkingPortrait, listeningPortrait;

            if (cCurrentLine.cCharacter.bTalkOnRightSide)
            {
                talkingPortrait = iRightCharacterPortrait;
                listeningPortrait = iLeftCharacterPortrait;
            }
            else
            {
                talkingPortrait = iLeftCharacterPortrait;
                listeningPortrait = iRightCharacterPortrait;
            }
            
            // Reactiver les portraits au cas ou ils etaient desactives
            iRightCharacterPortrait.gameObject.SetActive(true);
            iLeftCharacterPortrait.gameObject.SetActive(true);
            
            CopyImageProperties(cCurrentLine.cCharacter.iTalkingPortrait, talkingPortrait);
            CopyImageProperties(cCurrentLine.cCharacter.iListeningPortrait, listeningPortrait);
            
            StopAllCoroutines();
            StartCoroutine(CoroutineAnimatePortraitFocus(talkingPortrait, listeningPortrait));
        }
        
        StartCoroutine(CoroutineTypeSentence(cCurrentLine.sLine));
    }

    private void CopyImageProperties(Sprite source, Image target)
    {
        if (source == null || target == null || !target.gameObject.activeSelf)
            return;

        target.sprite = source;
        target.preserveAspect = true; 
    }

    private IEnumerator CoroutineAnimatePortraitFocus(Image talkingPortrait, Image listeningPortrait)
    {
        if (talkingPortrait == null || listeningPortrait == null || !talkingPortrait.gameObject.activeSelf || !listeningPortrait.gameObject.activeSelf)
            yield break;

        float duration = 0.3f;
        float elapsed = 0f;

        // Initial Values, to be reattributed later
        Vector3 talkingInitialScale = talkingPortrait.transform.localScale;
        Vector3 listeningInitialScale = listeningPortrait.transform.localScale;
        Color talkingInitialColor = talkingPortrait.color;
        Color listeningInitialColor = listeningPortrait.color;

        // Values applied during animation (To make it bigger/smaller)
        Vector3 talkingTargetScale = Vector3.one * 1.5f;
        Vector3 listeningTargetScale = Vector3.one * 0.75f;
        Color talkingTargetColor = Color.white;
        Color listeningTargetColor = new Color(0.7f, 0.7f, 0.7f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Animate without animator
            talkingPortrait.transform.localScale = Vector3.Lerp(talkingInitialScale, talkingTargetScale, t);
            talkingPortrait.color = Color.Lerp(talkingInitialColor, talkingTargetColor, t);

            listeningPortrait.transform.localScale = Vector3.Lerp(listeningInitialScale, listeningTargetScale, t);
            listeningPortrait.color = Color.Lerp(listeningInitialColor, listeningTargetColor, t);

            yield return null;
        }

        // Ensure perfect final values
        talkingPortrait.transform.localScale = talkingTargetScale;
        talkingPortrait.color = talkingTargetColor;
        listeningPortrait.transform.localScale = listeningTargetScale;
        listeningPortrait.color = listeningTargetColor;
    }
    
    IEnumerator CoroutineTypeSentence(string sSentence)
    {
        bIsTypingText = true;
        tDialogueDisplay.text = "";
        foreach (char cLetter in sSentence.ToCharArray())
        {
            tDialogueDisplay.text += cLetter;
            yield return new WaitForSeconds(m_currentTextSpeed);
        }
        bIsTypingText = false;
        m_currentTextSpeed = defaultTextSpeed;
    }

    public void EndDialogue()
    {
        bisDialogueActive = false;
        HideDialogueUI(0);
        
    }
}
