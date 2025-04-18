using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class SGL_InventoryManager : Singleton<SGL_InventoryManager>
{
    [Header("Inventory Content")]
    [SerializeField] private List<OBJ_Item> m_items = new();
    
    public void AddItem(OBJ_Item item)
    {
        m_items.Add(item);
        // Update the UI to reflect the addition
        UI_ManageInventory.Instance.UpdateGridElement(m_items.Count - 1, item);
        Debug.Log("Item added: " + item.name);
    }

    public bool CheckForItem(OBJ_Item item)
    {
        return m_items.Contains(item);
    }
    
    public void RemoveItem(OBJ_Item item)
    {
        if (m_items.Contains(item))
        {
            m_items.Remove(item);
            // Update the UI to reflect the removal
            UI_ManageInventory.Instance.UpdateContentPanel(m_items);
            Debug.Log("Item removed: " + item.name);
        }
        else
            Debug.Log("Item not found: " + item.name);
    }
    
    public void ClearInventory()
    {
        m_items.Clear();
        Debug.Log("Inventory cleared.");
        int allChildren = UI_ManageInventory.Instance.gContentPanel.GetComponentsInChildren<RectTransform>().Length;
        // Clear all UI elements
        for (int i = 0; i < allChildren; i++)
            UI_ManageInventory.Instance.ClearGridElement(i);
    }
    
    public void UpdateUI()
    {
        // Update the UI to reflect the current inventory state
        for (int i = 0; i < m_items.Count; i++)
        {
            // Assuming you have a method to update the UI for each item
            UI_ManageInventory.Instance.UpdateGridElement(i, m_items[i]);
        }
    }
}
