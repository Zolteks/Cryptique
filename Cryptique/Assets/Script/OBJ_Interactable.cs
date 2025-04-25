using UnityEngine;

public abstract class OBJ_Interactable : MonoBehaviour
{
    protected enum InteractMethod
    {
        None = 0,
        Walk,
        WalkAndInterract,
        Arrow,
    }

    /* Variables */
    [SerializeField] private bool m_canInteract = true;
    [SerializeField] protected InteractMethod m_interactMethod = InteractMethod.None;

    /* Functions */

    /// <summary>
    /// Indique si une interaction est possible.
    /// </summary>
    public bool CanInteract() { return m_canInteract; }

    /// <summary>
    /// Applique le booleen m_canInteract.
    /// </summary>
    /// <param name="canInteract">True si l'interaction est possible, sinon false.</param>
    public void SetCanInteract(bool canInteract) => m_canInteract = canInteract;

    /// <summary>  
    /// Interagit avec l'objet.  
    /// </summary>  
    /// <returns>True si l'interaction a reussi, sinon false.</returns>  
    public abstract bool Interact();

    public virtual void TriggerInteract()
    {
        switch (m_interactMethod)
        {
            case InteractMethod.None:
                Interact();
                break;
            case InteractMethod.Walk:
                PC_PlayerController.Instance.OnMoveCallback += InteractionCallback;
                PC_PlayerController.Instance.MoveTo();
                break;
            case InteractMethod.WalkAndInterract:
                PC_PlayerController.Instance.OnInteractionCallback += InteractionCallback;
                PC_PlayerController.Instance.MoveForInteraction();
                break;
            case InteractMethod.Arrow:
                PC_PlayerController.Instance.OnMoveCallback += InteractionCallback;
                break;
        }
    }

    protected void InteractionCallback()
    {
        switch (m_interactMethod)
        {
            case InteractMethod.Walk:
                PC_PlayerController.Instance.OnMoveCallback -= InteractionCallback;
                break;
            case InteractMethod.WalkAndInterract:
                PC_PlayerController.Instance.OnInteractionCallback -= InteractionCallback;
                break;
            case InteractMethod.Arrow:
                PC_PlayerController.Instance.OnMoveCallback -= InteractionCallback;
                break;
        }

        Interact();
    }
}