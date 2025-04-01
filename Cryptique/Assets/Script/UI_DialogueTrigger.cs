using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DialogueTrigger : MonoBehaviour
{

    [System.Serializable]
    public class DialogueCharacter
    {
        public string sName;
        public Image iTalkingPortrait;
        public Image iListeningPortrait;
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

    public Dialogue cDialogue;

    public void TriggerDialogue()
    {
        UI_DialogueManager.cInstance.StartDialogue(cDialogue);
    }   
}
