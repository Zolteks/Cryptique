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

    private Queue<DialogueLine> qLines;

    public bool isDialogueActive = false;

    public float fTextSpeed = 0.1f;

    //public Animator aDialogueAnimation;

    // Start is called before the first frame update
    void Start()
    {
        if (cInstance == null)
            cInstance = this;
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

        iRightCharacterPortrait.sprite = cCurrentLine.cCharacter.iTalkingPortrait.sprite;

        iLeftCharacterPortrait.sprite = cCurrentLine.cCharacter.iListeningPortrait.sprite;

        tNameText.text = cCurrentLine.cCharacter.sName;

        StopAllCoroutines();

        StartCoroutine(TypeSentence(cCurrentLine.sLine));
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
    }
}
