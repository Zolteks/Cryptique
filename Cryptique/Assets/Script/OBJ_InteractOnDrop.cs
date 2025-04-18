using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OBJ_InteractOnDrop : OBJ_Interactable
{
    /* Variables */
    [SerializeField] private List<OBJ_Item> m_itemToDrop;
    private OBJ_Item m_item;
    
    /* Getters and Setters */
    public List<OBJ_Item> GetItemToDrop() => m_itemToDrop;
    public OBJ_Item GetItemDropped() => m_item;

    /* Functions */
    public void UseItemOnDrop(OBJ_Item itemToDrop)
    {
        foreach (OBJ_Item item in m_itemToDrop)
        {
            // Check if the item to drop is the same as the one in the inventory
            if (itemToDrop != item)
            {
                continue;
            } else
            {
                Debug.Log("Used " + itemToDrop.name + " on " + gameObject.name);

                m_item = itemToDrop;

                // Remove from inventory
                SGL_InventoryManager.Instance.RemoveItem(itemToDrop);

                // Call the Interact method of the interactable object
                Interact();
            }
        }
    }
}
