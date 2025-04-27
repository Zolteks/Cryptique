using UnityEngine;

public class OBJ_Collectable : OBJ_Interactable
{
    /* Variables */
    [SerializeField] protected OBJ_Item m_item;

    /* Getters and Setters */
    public OBJ_Item GetItem() => m_item;

    /* Functions */
    public override bool Interact()
    {
        if (!CanInteract())
            return false;

        // Add to inventory
        SGL_InventoryManager.Instance.AddItem(m_item);

        // Notify the GameProgressionManager that an item as been collected
        GameProgressionManager.Instance.CollectItem(m_item.GetRegion() ,m_item.GetName());
        Debug.Log($"Item {m_item.name} collected ! add to GameProgressionManager ");

        Destroy(gameObject);
        return true;
    }
}