using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_DialogueTrigger : MonoBehaviour
{

    private void Start()
    {
        UI_DialogueManager.Instance.HideDialogueUI(0);
    }

    public void TriggerDialogue()
    {
        UI_DialogueManager.Instance.ShowDialogueUI();
        //UI_DialogueManager.cInstance.StartDialogue();
    }   
}
