using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;


public class IN_Character : OBJ_Interactable
{
    /* Serialized Fields */
    [Header("Path Settings")]
    [SerializeField] List<Transform> m_crossingPoints;
    [Header("Dialogue Settings")]
    [SerializeField] private SharedTableData m_localizationAsset;
    [SerializeField] private DialogueCharacterList m_characterList;
    [Header("Bubble Settings")]
    [SerializeField] private Sprite m_dialogueBubble;
    [SerializeField] [Range(0.1f, 1f)] private float m_scale = 0.6f;
    
    /* Variables */
    private Camera m_camera;
    private UI_DialogueManager.Dialogue m_dialogue;
    private NavMeshAgent m_navMeshAgent;
    private int m_pointIndex = 0;
    private GameObject m_bubbleObject;
    
    /* Singletons */
    private PC_PlayerController m_playerController;
    private LanguageManager m_languageManager;
    private UI_DialogueManager m_dialogueManager;

    private bool wasInteracting = false;

    /* Functions */
    private void Start()
    {
        m_navMeshAgent = GetComponent<NavMeshAgent>();
        if(m_navMeshAgent == null)
            Debug.LogWarning("A NPC doesn't have navmesh agent");
        m_camera = Camera.main;
        if (m_camera == null)
            Debug.LogError("Camera not found");
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
        ConstructBubble();
    }

    private void OnDestroy()
    {
        if (m_bubbleObject != null)
            Destroy(m_bubbleObject);
    }

    private void LateUpdate()
    {
        if (m_dialogueBubble == null || m_bubbleObject == null)
            return;
        
        Vector3 cameraPosition = m_camera.transform.position;
        cameraPosition.y = m_bubbleObject.transform.position.y;
        m_bubbleObject.transform.LookAt(cameraPosition);
        m_bubbleObject.transform.Rotate(0f, 180f, 0f);
    }

    public override bool Interact()
    {
        Wait();
        wasInteracting = true;
        return true;
    }
    
    private void ConstructBubble()
    {
        if (m_dialogueBubble == null)
            return;

        var collider = GetComponent<BoxCollider>();
        if (collider == null)
        {
            Debug.LogError("A NPC doesn't have a collider");
            return;
        }
        // Créer un GameObject enfant
        m_bubbleObject = new GameObject("DialogueBubble");
        m_bubbleObject.transform.SetParent(transform);
        float imageWidthOffset = m_dialogueBubble.bounds.size.x / 2 * m_scale;
        float imageHeightOffset = m_dialogueBubble.bounds.size.y / 6 * m_scale;
        m_bubbleObject.transform.localPosition = new Vector3(imageWidthOffset, collider.bounds.size.y + imageHeightOffset, 0);
        m_bubbleObject.transform.localScale = new Vector3(m_scale, m_scale, m_scale);
        
        // Ajouter un SpriteRenderer et assigner le sprite
        SpriteRenderer spriteRenderer = m_bubbleObject.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = m_dialogueBubble;
        spriteRenderer.sortingOrder = 1; // Assurez-vous que l'ordre d'affichage est correct
    }
    
    private void Wait()
    {
        m_dialogueManager.ShowDialogueUI();
        m_dialogueManager.StartDialogue(m_dialogue);
    }
    
    private void ConstructDialogue()
    {
        if(m_characterList == null || m_localizationAsset == null)
            Debug.LogError("A NPC has no character list nor localized dialogue table");
        
        var currentLanguage = m_languageManager.GetCurrentLanguage();
        var localizedTable = LocalizationSettings.Instance.GetStringDatabase().GetTable(m_localizationAsset.TableCollectionName, LocalizationSettings.Instance.GetSelectedLocale());
        
        m_dialogue = new();
        for(int i = 0; i < m_characterList.talkingCharacters.Count; i++)
        {
            UI_DialogueManager.DialogueCharacter lineCharacter = new();
            var characterEntry = m_characterList.talkingCharacters[i];
            lineCharacter.bTalkOnRightSide = characterEntry.bTalkOnRightSide;
            lineCharacter.bNoPortrait = characterEntry.bNoPortrait;
            lineCharacter.iTalkingPortrait = characterEntry.iTalkingPortrait;
            lineCharacter.iListeningPortrait = characterEntry.iListeningPortrait;
            lineCharacter.sNameEN = characterEntry.sNameEN;
            lineCharacter.sNameFR = characterEntry.sNameFR;

            if(currentLanguage == LanguageCode.FR)
            {
                lineCharacter.sNameDisplay = characterEntry.sNameFR;
            }
            else if (currentLanguage == LanguageCode.EN)
            {
                lineCharacter.sNameDisplay = characterEntry.sNameEN;
            }
            else
            {
                lineCharacter.sNameDisplay = characterEntry.sNameEN;
            }

            UI_DialogueManager.DialogueLine line = new();
            var entry = localizedTable.GetEntry(i.ToString());
            line.sLine = entry.GetLocalizedString();
            line.cCharacter = lineCharacter;

            m_dialogue.lDialogueLines.Add(line);
        }
    }
    
    #region Mouvements
    public void GoToNextPoint()
    {
        if (m_pointIndex >= m_crossingPoints.Count - 1)
        {
            Debug.LogWarning("A character tried to go to an out of range position");
            return;
        }

        ++m_pointIndex;
        NavMesh.SamplePosition(m_crossingPoints[m_pointIndex].position, out NavMeshHit hit, 5, NavMesh.AllAreas);
        m_navMeshAgent.SetDestination(hit.position);
    }

    public void GoToPreviousPoint()
    {
        if (m_pointIndex <= 0)
        {
            Debug.LogWarning("A character tried to go to an out of range position");
            return;
        }

        --m_pointIndex;
        NavMesh.SamplePosition(m_crossingPoints[m_pointIndex].position, out NavMeshHit hit, 5, NavMesh.AllAreas);
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
            int gap = destination - m_pointIndex;
            Action incrementFunc = gap < 0 ? GoToPreviousPoint : GoToNextPoint;

            StartCoroutine(CoroutineGoToPointLoop(incrementFunc, Math.Abs(gap) - 1));
        }
        else
        {
            m_pointIndex = destination;
            NavMesh.SamplePosition(m_crossingPoints[m_pointIndex].position, out NavMeshHit hit, 5, NavMesh.AllAreas);
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
    
    public bool getWasInteracting()
    {
        return wasInteracting;
    }
}
