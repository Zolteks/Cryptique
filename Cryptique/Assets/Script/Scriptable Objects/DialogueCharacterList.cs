using System;
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
        public string sNameFR;
        public string sNameEN;
        [NonSerialized]public string sNameDisplay;
        public Sprite iTalkingPortrait;
        public Sprite iListeningPortrait;
        public bool bTalkOnRightSide;
        public bool bNoPortrait;
    }

    [SerializeField] public List<DialogueCharacter> talkingCharacters;
}
