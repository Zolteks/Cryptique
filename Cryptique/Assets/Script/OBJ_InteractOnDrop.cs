using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OBJ_InteractOnDrop : OBJ_Interactable
{
    /* Variables */
    [SerializeField] private OBJ_Item m_itemToDrop;
    
    /* Getters and Setters */
    public OBJ_Item GetItemToDrop() => m_itemToDrop;
    
    /* Functions */
    public void UseItemOnDrop(OBJ_Item itemToDrop)
    {
        // Check if the item to drop is the same as the one in the inventory
        if (itemToDrop != m_itemToDrop)
            return;

        Debug.Log("Used " + itemToDrop.name + " on " + gameObject.name);
        
        // Remove from inventory
        InventoryManager.Instance.RemoveItem(itemToDrop);
        
        // Call the Interact method of the interactable object
        Interact();
    }
}
