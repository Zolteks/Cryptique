using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OBJ_Collectable : MonoBehaviour, IInteractable
{
    /* Variables */
    public bool bCanInteract { get; protected set; } = true;
    [SerializeField] private GameObject m_objInteractable;

    /* Functions */
    public bool Interact()
    {
        if (!bCanInteract)
            return false;

        // Add to inventory
        Debug.Log("Collected " + gameObject.name);
        Destroy(gameObject);
        return true;
    }

    public void UseOn(GameObject ObjectToInteract)
    {
        // Use item on OBJ_Interactable
        if (!bCanInteract)
            return;

        if (ObjectToInteract != m_objInteractable.gameObject)
            return;

        // Use item on ObjectToInteract
        if (!ObjectToInteract.TryGetComponent(out IInteractable interactable))
            return;
        
        if (!interactable.bCanInteract)
            return;

        Debug.Log("Used " + gameObject.name + " on " + ObjectToInteract.name);
        interactable.Interact();
        Destroy(gameObject);
    }
}