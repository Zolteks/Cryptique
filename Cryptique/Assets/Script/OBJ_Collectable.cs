using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OBJ_Collectable : OBJ_Interactable
{
    /* Variables */
    [SerializeField] private GameObject m_objInteractable;

    /* Functions */
    public override bool Interact()
    {
        if (!CanInteract())
            return false;

        // Add to inventory
        Debug.Log("Collected " + gameObject.name);
        Destroy(gameObject);
        return true;
    }

    public void UseOn(GameObject ObjectToInteract)
    {
        // Use item on OBJ_Interactable
        if (!CanInteract())
            return;

        if (ObjectToInteract != m_objInteractable.gameObject)
            return;

        // Use item on objInteractable
        if (!TryGetComponent<OBJ_Interactable>(out var objInteractable))
            return;

        Debug.Log("Used " + gameObject.name + " on " + ObjectToInteract.name);
        objInteractable.Interact();
        Destroy(gameObject);
    }
}