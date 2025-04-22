using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IN_Bush : OBJ_Interactable
{

    private bool isInteract;

    public override bool Interact()
    {
        if (false == CanInteract())
            return false;

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            return false;

        isInteract = true;

        return true;
    }

    public bool getIsInteract()
    {
        return isInteract;
    }
}
