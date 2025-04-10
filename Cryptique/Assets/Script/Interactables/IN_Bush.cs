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

        return true;
    }

    private void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;
        if(Interact())
            isInteract = true;
    }

    public bool getIsInteract()
    {
        return isInteract;
    }
}
