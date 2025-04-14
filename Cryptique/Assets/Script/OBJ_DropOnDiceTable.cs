using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OBJ_DropOnDiceTable : OBJ_InteractOnDrop
{
    [NonSerialized] static public bool playerHasDice = false;
    public override bool Interact()
    {
        playerHasDice = true;
        Destroy(this);
        return true;
    }
}
