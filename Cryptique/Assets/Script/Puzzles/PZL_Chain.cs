using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_Chain : OBJ_InteractOnDrop
{
    [SerializeField] private PZL_ChainComplete PZL_ChainComplete;

    public override bool Interact()
    {
        Destroy(gameObject);
        PZL_ChainComplete.CompleteChainPuzzle();
        return true;
    }
}
