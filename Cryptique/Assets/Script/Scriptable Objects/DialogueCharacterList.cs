using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/DialogueLine", order = 1)]
public class DialogueCharacterList : ScriptableObject
{
    [System.Serializable]
    public class DialogueCharacter
    {
        public string sName;
        public Image iTalkingPortrait;
        public Image iListeningPortrait;
        public bool bTalkOnRightSide;
    }

    [SerializeField] public List<DialogueCharacter> talkingCharacters;
}
