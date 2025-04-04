using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OBJ_Collectable : OBJ_Interactable
{
    /* Variables */
    [SerializeField] private GameObject m_objInteractable;
    [SerializeField] private OBJ_Item m_item;


    /* Getters and Setters */
    public OBJ_Item GetItem() => m_item;


    /* Functions */
    public override bool Interact()
    {
        if (!CanInteract())
            return false;

        // Add to inventory
        InventoryManager.Instance.AddItem(m_item);

        // Notify the GameProgressionManager that an item as been collected
        GameProgressionManager.Instance.CollectItem(m_item.GetRegion() ,m_item.GetName());
        Debug.Log($"Item {m_item.name} collected ! add to GameProgressionManager ");


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