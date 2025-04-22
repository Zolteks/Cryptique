using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;


public class IN_Character : OBJ_Interactable
{
    [SerializeField] string m_sName;
    [SerializeField] List<Transform> m_crossingPoints;

    NavMeshAgent m_navMeshAgent;
    int m_iPointIndex = 0;

    UI_DialogueManager.Dialogue m_dialogue;
    [SerializeField] SharedTableData m_localizationAsset;
    [SerializeField] DialogueCharacterList m_characterList;

    private PC_PlayerController m_playerController;



    private void Start()
    {
        if(false == TryGetComponent<NavMeshAgent>(out m_navMeshAgent))
        {
            Debug.LogWarning("A NPC doesn't have navmesh agent");
        }

        LocalizationSettings.Instance.OnSelectedLocaleChanged += (Locale) => ConstructDialogue();

        m_playerController = PC_PlayerController.Instance;
        ConstructDialogue();
    }

    public override bool Interact()
    {
        m_playerController.MoveForInteraction();
        m_playerController.OnInteractionCallback += Wait;

        //throw new NotImplementedException();
        return true;
    }

    void Wait()
    {
        UI_DialogueManager.Instance.ShowDialogueUI();
        UI_DialogueManager.Instance.StartDialogue(m_dialogue);

        m_playerController.OnInteractionCallback -= Wait;
    }

    void ConstructDialogue()
    {
        if(m_characterList == null || m_localizationAsset == null)
        {
            Debug.LogError("A NPC has no character list nor localized dialogue table");
        }
        LanguageCode currentLanguage = LanguageManager.Instance.GetCurrentLanguage();

        StringTable localizedTable = LocalizationSettings.Instance.GetStringDatabase().GetTable(m_localizationAsset.TableCollectionName, LocalizationSettings.Instance.GetSelectedLocale());
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

    #region Mouvements
    public void GoToNextPoint()
    {
        if (m_iPointIndex >= m_crossingPoints.Count - 1)
        {
            Debug.LogWarning("A character tried to go to an out of range position");
            return;
        }

        ++m_iPointIndex;
        NavMesh.SamplePosition(m_crossingPoints[m_iPointIndex].position, out NavMeshHit hit, 5, NavMesh.AllAreas);
        m_navMeshAgent.SetDestination(hit.position);
    }

    public void GoToPreviousPoint()
    {
        if (m_iPointIndex <= 0)
        {
            Debug.LogWarning("A character tried to go to an out of range position");
            return;
        }

        --m_iPointIndex;
        NavMesh.SamplePosition(m_crossingPoints[m_iPointIndex].position, out NavMeshHit hit, 5, NavMesh.AllAreas);
        m_navMeshAgent.SetDestination(hit.position);
    }

    public void GoToSpecificPoint(int destination, bool followingWolePath = true)
    {
        if (destination >= m_crossingPoints.Count)
        {
            Debug.LogWarning("A character tried to go to an out of range position");
            return;
        }

        if (followingWolePath)
        {
            int gap = destination - m_iPointIndex;
            Action incrementFunc = gap < 0 ? GoToPreviousPoint : GoToNextPoint;

            StartCoroutine(CoroutineGoToPointLoop(incrementFunc, Math.Abs(gap) - 1));
        }
        else
        {
            m_iPointIndex = destination;
            NavMesh.SamplePosition(m_crossingPoints[m_iPointIndex].position, out NavMeshHit hit, 5, NavMesh.AllAreas);
            m_navMeshAgent.SetDestination(hit.position);
        }
    }

    private IEnumerator CoroutineGoToPointLoop(Action incrementFunc, int loopAmount)
    {
        incrementFunc();
        Vector3 pathGap = transform.position - m_navMeshAgent.destination;
        pathGap.y = 0;
        while (pathGap.magnitude >= .1)
        {
            pathGap = transform.position - m_navMeshAgent.destination;
            pathGap.y = 0;
            yield return null;
        }
        
        if (loopAmount > 0)
        {
            StartCoroutine(CoroutineGoToPointLoop(incrementFunc, Math.Abs(loopAmount - 1)));
        }
    }

    public void FollowAlongPoints()
    {
        GoToSpecificPoint(m_crossingPoints.Count - 1);
    }

    #endregion Mouvements

    private void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    GoToPreviousPoint();
        //}
        //if (Input.GetMouseButtonDown(1))
        //{
        //    GoToNextPoint();
        //}
        //if (Input.GetMouseButtonDown(2))
        //{ 
        //    GoToSpecificPoint(0);
        //}
    }
    //private void OnMouseDown()
    //{
    //    Debug.Log(GameObject.Find("EventSystem"));
    //    if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

    //    if (CanInteract())
    //    {
    //        Interact();
    //    }
    //}
}
