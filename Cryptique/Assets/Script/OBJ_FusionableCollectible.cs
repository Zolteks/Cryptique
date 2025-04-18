using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class OBJ_FusionableCollectible : OBJ_Collectable
{
    [SerializeField] FusionHolder fusionHolder;

    public new bool Interact()
    {
        if (!CanInteract())
            return false;

        // Add to inventory
        SGL_InventoryManager.Instance.AddItem(m_item);

        // Notify the GameProgressionManager that an item as been collected
        GameProgressionManager.Instance.CollectItem(m_item.GetRegion(), m_item.GetName());
        Debug.Log($"Item {m_item.name} collected ! add to GameProgressionManager ");


        foreach(var item in fusionHolder.items)
        {
            if(false == SGL_InventoryManager.Instance.CheckForItem(item))
            {
                Destroy(gameObject);
                return true;
            }
        }

        // If we're still here, it means the player has all objects needed to fusion
        foreach (var item in fusionHolder.items)
        {
            SGL_InventoryManager.Instance.RemoveItem(item);
        }

        SGL_InventoryManager.Instance.AddItem(fusionHolder.reward);

        Destroy(gameObject);
        return true;
    }
}
