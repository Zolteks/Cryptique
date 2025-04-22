using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PZL_BreakIcePotion : OBJ_InteractOnDrop
{
    public override bool Interact()
    {
        OBJ_Item m_item = GetItemDropped();

        if (m_item.name == "PotionFailed")
        {
            print("failed");
        } 
        else
        {
            if (m_item.name == "FireResistance")
            {
                print("successfull");
                Destroy(gameObject);
            }
        }
        return true;
    }
}
