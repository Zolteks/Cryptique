using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OBJ_Collectable : OBJ_Interactable
{
    /* Variables */
    [SerializeField] private GameObject m_objInteractable;
    [SerializeField] private OBJ_Item m_item;

    /* Functions */
    public override bool Interact()
    {
        if (!CanInteract())
            return false;

        // Add to inventory
        InventoryManager.Instance.AddItem(m_item);
        Destroy(gameObject);
        return true;
    }

    public void UseOn(GameObject ObjectToInteract)
    {
        // Use item on a specific object
        if (!CanInteract())
            return;

        if (ObjectToInteract != m_objInteractable.gameObject)
            return;

        // Use item on objInteractable
        if (!TryGetComponent<OBJ_Interactable>(out var objInteractable))
            return;

        Debug.Log("Used " + gameObject.name + " on " + ObjectToInteract.name);
        // Call the Interact method of the interactable object
        objInteractable.Interact();
        // Remove from inventory
        InventoryManager.Instance.RemoveItem(m_item);
        Destroy(gameObject);
    }
}