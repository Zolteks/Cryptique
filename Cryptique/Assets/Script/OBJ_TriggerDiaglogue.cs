using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using UnityEngine.Serialization;

public class OBJ_TriggerDiaglogue : MonoBehaviour
{
    private BoxCollider m_collider;
    private PC_PlayerController m_playerController;
    private LanguageManager m_languageManager;
    private UI_DialogueManager m_dialogueManager;
    
    UI_DialogueManager.Dialogue m_dialogue;
    [Header("Dialogue Settings")]
    [SerializeField] DialogueCharacterList m_characterList;
    [SerializeField] SharedTableData m_dialogueLocalizationTable;
    
    void Start()
    {
        m_collider = GetComponent<BoxCollider>();
        if (m_collider == null)
            Debug.LogError("BoxCollider not found");
        m_playerController = PC_PlayerController.Instance;
        if (m_playerController == null)
            Debug.LogError("PlayerController not found");
        m_languageManager = LanguageManager.Instance;
        if (m_languageManager == null)
            Debug.LogError("LanguageManager not found");
        m_dialogueManager = UI_DialogueManager.Instance;
        if (m_dialogueManager == null)
            Debug.LogError("DialogueManager not found");
        LocalizationSettings.Instance.OnSelectedLocaleChanged += (locale) => ConstructDialogue();
        ConstructDialogue();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TriggerEnter with : " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            m_playerController.StopMovement();
            m_dialogueManager.ShowDialogueUI();
            m_dialogueManager.StartDialogue(m_dialogue);
            m_collider.enabled = false;
        }
    }
    
    void ConstructDialogue()
    {
        if(m_characterList == null || m_dialogueLocalizationTable == null)
        {
            Debug.LogError("A NPC has no character list nor localized dialogue table");
        }
        LanguageCode currentLanguage = m_languageManager.GetCurrentLanguage();

        StringTable localizedTable = LocalizationSettings.Instance.GetStringDatabase().GetTable(m_dialogueLocalizationTable.TableCollectionName, LocalizationSettings.Instance.GetSelectedLocale());
        m_dialogue = new();

        for(int i=0; i< m_characterList.talkingCharacters.Count; i++)
        {
            UI_DialogueManager.DialogueCharacter lineChatracter = new();
            var characterEntry = m_characterList.talkingCharacters[i];
            lineChatracter.bTalkOnRightSide = characterEntry.bTalkOnRightSide;
            lineChatracter.bNoPortrait = characterEntry.bNoPortrait;
            lineChatracter.iTalkingPortrait = characterEntry.iTalkingPortrait;
            lineChatracter.iListeningPortrait = characterEntry.iListeningPortrait;
            lineChatracter.sNameEN = characterEntry.sNameEN;
            lineChatracter.sNameFR = characterEntry.sNameFR;

            if(currentLanguage == LanguageCode.FR)
            {
                lineChatracter.sNameDisplay = characterEntry.sNameFR;
            }
            else if (currentLanguage == LanguageCode.EN)
            {
                lineChatracter.sNameDisplay = characterEntry.sNameEN;
            }
            else
            {
                lineChatracter.sNameDisplay = characterEntry.sNameEN;
            }

            UI_DialogueManager.DialogueLine line = new();
            var entry = localizedTable.GetEntry(i.ToString());
            line.sLine = entry.GetLocalizedString();
            line.cCharacter = lineChatracter;

            m_dialogue.lDialogueLines.Add(line);
        }
    }
}
