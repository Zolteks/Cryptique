using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class OBJ_Interactable : MonoBehaviour
{
    /* Variables */
    [SerializeField] private bool m_canInteract = true;
    [SerializeField] protected bool m_walkToItem = false;

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
        if (m_walkToItem && Vector3.Distance(PC_PlayerController.Instance.transform.position, transform.position) < 100)
        {
            PC_PlayerController.Instance.OnMoveCallback += InteractionCallback;
        }
        else
            Interact();
    }

    protected void InteractionCallback()
    {
        Interact();
        PC_PlayerController.Instance.OnMoveCallback -= InteractionCallback;
    }
}