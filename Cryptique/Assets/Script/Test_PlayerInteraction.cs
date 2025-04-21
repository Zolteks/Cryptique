using UnityEngine;

public class Test_PlayerInteraction : OBJ_Interactable
{
    [SerializeField] private PC_PlayerController m_playerController;
    
    private void Awake()
    {
        m_playerController = PC_PlayerController.Instance;
        if (m_playerController == null)
            Debug.LogError("PlayerController not found");
    }

    private void OnEnable()
    {
        m_playerController.OnInteractionCallback += EndOfInteraction;
    }
    
    private void OnDisable()
    {
        m_playerController.OnInteractionCallback -= EndOfInteraction;
    }
    
    public override bool Interact()
    {
        if (!m_playerController.GetInputActive())
            return false;
        Debug.Log("Launched Player Interact");
        m_playerController.MoveForInteraction();
        return true;
    }

    private void EndOfInteraction()
    {
        Debug.Log("End of interaction");
    }
}
