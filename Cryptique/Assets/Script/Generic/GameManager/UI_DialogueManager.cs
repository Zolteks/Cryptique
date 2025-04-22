using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;
using static UI_DialogueTrigger;

public class UI_DialogueManager : Singleton<UI_DialogueManager>
{

    /* Singleton */
    private static UI_DialogueManager m_DialogueManager;

    #region Serializable Class
    [System.Serializable]
    public class DialogueCharacter
    {
        public string sNameEN;
        public string sNameFR;
        public string sNameDisplay;
        public Image iTalkingPortrait;
        public Image iListeningPortrait;
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

    LanguageManager languageManager;

    //public Animator dialogueAnimator;

    private Queue<DialogueLine> qLines;

    public bool bisDialogueActive = false;

    public float fTextSpeed = 0.05f;

    private bool bIsTypingText = false;

    //public Animator aDialogueAnimation;

    void Awake()
    {
        m_DialogueManager = UI_DialogueManager.Instance;
        if(m_DialogueManager == null)
        {
            Debug.LogError("DialogueManager is null");
        }

        languageManager = LanguageManager.Instance;
        if(languageManager == null)
        {
            Debug.LogError("LanguageManager is null");
        }

        HideDialogueUI(0);
    }

    private void Start()
    {
        iRightCharacterPortrait = GameObject.Find("Dialog_RightPortrait").GetComponent<Image>();
        iLeftCharacterPortrait = GameObject.Find("Dialog_LeftPortrait").GetComponent<Image>();
        tNameText = GameObject.Find("Dialog_TalkName").GetComponent<TextMeshProUGUI>();
        tDialogueDisplay = GameObject.Find("Dialog_TextContent").GetComponent<TextMeshProUGUI>();
        dialoguePanel = GameObject.Find("Dialogue_box");
    }

    public void ShowDialogueUI()
    {
        PC_PlayerController.Instance.DisableInput();
        //SGL_InteractManager.Instance.EnableInteraction();
        dialoguePanel.SetActive(true);
        //dialogueAnimator.SetTrigger("Show");
    }

    public void HideDialogueUI(float time)
    {
        //dialogueAnimator.SetTrigger("Hide");
        StartCoroutine(CoroutineDeactivateAfterAnimation(time));
    }

    IEnumerator CoroutineDeactivateAfterAnimation(float time)
    {
        yield return new WaitForSeconds(time); // Animation Length
        dialoguePanel.SetActive(false);
        bisDialogueActive = false;
        PC_PlayerController.Instance.EnableInput();
    }

    public void StartDialogue(Dialogue c_Dialogue)
    {
        bisDialogueActive = true;

        //aDialogueAnimation.Play("show");

        if (qLines == null)
        {
            qLines = new Queue<DialogueLine>();
        }
        else
        {
            qLines.Clear();
        }
            

        foreach (DialogueLine cLine in c_Dialogue.lDialogueLines)
        {
            LanguageCode currentLanguage = LanguageManager.Instance.GetCurrentLanguage();

            qLines.Enqueue(cLine);
        }

        DisplayNextLine();
    }

    public void ButtonDisplayNextLine()
    {
        UI_DialogueManager.Instance.DisplayNextLine();
    }
    
    public void DisplayNextLine()
    {
        if (bIsTypingText == true)
        {
            fTextSpeed = 0.0001f;
            return;
        }

        if (qLines.Count == 0)
        {
            EndDialogue();
            return;
        }
        else
        {
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

                // Réactiver les portraits au cas où ils étaient désactivés
                iRightCharacterPortrait.gameObject.SetActive(true);
                iLeftCharacterPortrait.gameObject.SetActive(true);

                CopyImageProperties(cCurrentLine.cCharacter.iTalkingPortrait, talkingPortrait);
                CopyImageProperties(cCurrentLine.cCharacter.iListeningPortrait, listeningPortrait);

                StopAllCoroutines();
                StartCoroutine(CoroutineAnimatePortraitFocus(talkingPortrait, listeningPortrait));
            }

            StartCoroutine(CoroutineTypeSentence(cCurrentLine.sLine));
        }
    }

    private void CopyImageProperties(Image source, Image target)
    {
        if (source == null || target == null || !target.gameObject.activeSelf)
            return;

        target.sprite = source.sprite;
        target.color = source.color;
        target.preserveAspect = source.preserveAspect;
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
            yield return new WaitForSeconds(fTextSpeed);
        }
        bIsTypingText = false;
        fTextSpeed = 0.05f;
    }

    public void EndDialogue()
    {
        bisDialogueActive = false;
        //aDialogueAnimation.Play("hide");
        HideDialogueUI(0.5f);
    }
}
