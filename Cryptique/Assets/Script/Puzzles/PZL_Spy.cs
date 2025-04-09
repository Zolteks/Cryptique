using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public class PZL_Spy : Puzzle
{
    [SerializeField] private Transform lastTile;
    [SerializeField] private GameObject player;


    UI_DialogueManager.Dialogue m_dialogue;
    [SerializeField] SharedTableData m_localizationAsset;
    [SerializeField] DialogueCharacterList m_characterList;

    [SerializeField] private Gyroscope gyroscope;

    private bool isStarted = false;


    private void Awake()
    {
    }

    private void Start()
    {
        LocalizationSettings.Instance.OnSelectedLocaleChanged += (Locale) => ConstructDialogue();
        ConstructDialogue();
    }

    void ConstructDialogue()
    {
        if (m_characterList == null || m_localizationAsset == null)
        {
            Debug.LogError("A NPC has no character list nor localized dialogue table");
        }

        StringTable localizedTable = LocalizationSettings.Instance.GetStringDatabase().GetTable(m_localizationAsset.TableCollectionName, LocalizationSettings.Instance.GetSelectedLocale());
        m_dialogue = new();

        for (int i = 0; i < m_characterList.talkingCharacters.Count; i++)
        {
            UI_DialogueManager.DialogueCharacter lineChatracter = new();
            var characterEntry = m_characterList.talkingCharacters[i];
            lineChatracter.bTalkOnRightSide = characterEntry.bTalkOnRightSide;
            lineChatracter.iTalkingPortrait = characterEntry.iTalkingPortrait;
            lineChatracter.iListeningPortrait = characterEntry.iListeningPortrait;
            lineChatracter.sName = characterEntry.sName;


            UI_DialogueManager.DialogueLine line = new();
            var entry = localizedTable.GetEntry(i.ToString());
            line.sLine = entry.GetLocalizedString();
            line.cCharacter = lineChatracter;

            m_dialogue.lDialogueLines.Add(line);
        }
    }

    public void Update()
    {
        // Input de test
        if (Input.GetMouseButtonDown(0))
        {
            gyroscope.enabled = true;
            Debug.Log("Gyroscope enabled");
        }

        if (gyroscope.isCalibrated && !isStarted)
        {
            PlayDialogue();
        }

        StartCoroutine(WaitForSeconds());
        
        if (!UI_DialogueManager.cInstance.bisDialogueActive  && isStarted)
        {
            Complete();
        }

        //Simulation mouvement
        if (Input.GetMouseButtonDown(1))
        {
            isDetectMovement();
            Debug.Log("Movement detected");
        }
    }

    public void PlayDialogue()
    {
        UI_DialogueManager.cInstance.ShowDialogueUI();
        UI_DialogueManager.cInstance.StartDialogue(m_dialogue);

        isStarted = true;
        Debug.Log("Dialogue started");
    }

    public void isDetectMovement()
    {
        // Force the end of the dialogue and force the player to back to the start of the puzzle
        UI_DialogueManager.cInstance.EndDialogue();

        //TODO: Add a transition screen
        player.transform.position = lastTile.position;
    }

    public void Success()
    {
        Debug.Log($"Puzzle completed successfully.");
    }


    //    public void OnTriggerEnter(Collider other)
    //    {
    //        if (other.CompareTag("Player"))
    //        {
    //            gyroscope.enabled = true;
    //        }
    //    }

    private IEnumerator WaitForSeconds()
    {
        if(isStarted)
        {
            yield return new WaitForSeconds(0.5f);
            UI_DialogueManager.cInstance.DisplayNextLine();
        }
    }

}

