using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonLauncher : OBJ_Interactable
{
    public override bool Interact()
    {
        var pzl = GetComponent<PZL_Simon>();
        pzl.ResetPuzzle();
        pzl.ChangeCamState();
        GetComponent<BoxCollider>().enabled = false;
        return true;
    }
}
