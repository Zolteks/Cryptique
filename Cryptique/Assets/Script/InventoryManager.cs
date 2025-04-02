using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    
    [SerializeField]
    private List<OBJ_Item> m_items = new List<OBJ_Item>();
    
    private void Awake()
    {
        Instance = this;
    }
    
    public void AddItem(OBJ_Item item)
    {
        m_items.Add(item);
        Debug.Log("Item added: " + item.name);
    }
    
    public void RemoveItem(OBJ_Item item)
    {
        if (m_items.Contains(item))
        {
            m_items.Remove(item);
            Debug.Log("Item removed: " + item.name);
        }
        else
            Debug.Log("Item not found: " + item.name);
    }
    
    public void ClearInventory()
    {
        m_items.Clear();
        Debug.Log("Inventory cleared.");
    }
    
    public void ShowInventory()
    {
        Debug.Log("Inventory contains:");
        foreach (var item in m_items)
        {
            Debug.Log(item.name);
        }
    }
}
