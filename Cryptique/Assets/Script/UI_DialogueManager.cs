using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UI_DialogueTrigger;

public class UI_DialogueManager : MonoBehaviour
{

    public static UI_DialogueManager cInstance;

    public Image iRightCharacterPortrait;
    public Image iLeftCharacterPortrait;
    public TextMeshProUGUI tNameText;
    public TextMeshProUGUI tDialogueDisplay;

    public GameObject dialoguePanel;

    //public Animator dialogueAnimator;

    private Queue<DialogueLine> qLines;

    public bool isDialogueActive = false;

    public float fTextSpeed = 0.1f;

    //public Animator aDialogueAnimation;

    // Start is called before the first frame update
    void Start()
    {
        if (cInstance == null)
            cInstance = this;

        HideDialogueUI();
    }

    public void ShowDialogueUI()
    {
        dialoguePanel.SetActive(true);
        //dialogueAnimator.SetTrigger("Show");
    }

    public void HideDialogueUI()
    {
        //dialogueAnimator.SetTrigger("Hide");
        StartCoroutine(DeactivateAfterAnimation());
    }

    IEnumerator DeactivateAfterAnimation()
    {
        yield return new WaitForSeconds(0.5f); // Animation Length
        dialoguePanel.SetActive(false);
        isDialogueActive = false;
    }

    public void StartDialogue(Dialogue c_Dialogue)
    {
        isDialogueActive = true;

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
            qLines.Enqueue(cLine);
        }

        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (qLines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine cCurrentLine = qLines.Dequeue();
        tNameText.text = cCurrentLine.cCharacter.sName;

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

        CopyImageProperties(cCurrentLine.cCharacter.iTalkingPortrait, talkingPortrait);
        CopyImageProperties(cCurrentLine.cCharacter.iListeningPortrait, listeningPortrait);

        StopAllCoroutines();
        StartCoroutine(AnimatePortraitFocus(talkingPortrait, listeningPortrait));
        StartCoroutine(TypeSentence(cCurrentLine.sLine));
    }

    private void CopyImageProperties(Image source, Image target)
    {
        if (source == null || target == null) return;

        target.sprite = source.sprite;
        target.color = source.color;
        target.preserveAspect = source.preserveAspect;
    }

    private IEnumerator AnimatePortraitFocus(Image talkingPortrait, Image listeningPortrait)
    {
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


    IEnumerator TypeSentence(string sSentence)
    {
        tDialogueDisplay.text = "";
        foreach (char cLetter in sSentence.ToCharArray())
        {
            tDialogueDisplay.text += cLetter;
            yield return new WaitForSeconds(fTextSpeed);
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        //aDialogueAnimation.Play("hide");
        HideDialogueUI();
    }
}
