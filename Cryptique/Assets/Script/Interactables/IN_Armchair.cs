using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IN_Armchair : OBJ_Interactable
{
    override public bool Interact()
    {
        GetComponent<PZL_Armchair>().DoComplete();

        return true;
    }

    //private void OnMouseDown()
    //{
    //    if (false == CanInteract()) return;

    //    Interact();
    //}
}
