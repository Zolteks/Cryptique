using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_RepairLadder : OBJ_InteractOnDrop
{
    private PZL_LadderComplete m_complete;

    private void Start()
    {
        m_complete = GetComponent<PZL_LadderComplete>();
    }

    public override bool Interact()
    {
        if(GetItemDropped().name == "Ladder")
        {
            m_complete.LadderComplete();
        }
        return true;
    }
}
