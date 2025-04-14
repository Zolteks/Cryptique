using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_Chain : OBJ_InteractOnDrop
{

    public override bool Interact()
    {
        Destroy(gameObject);
        GetComponent<PZL_ChainComplete>().CompleteChainPuzzle();
        return true;
    }
}
