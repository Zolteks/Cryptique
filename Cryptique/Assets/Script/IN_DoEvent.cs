using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IN_DoEvent : OBJ_Interactable
{
    [SerializeField] UnityEvent OnInteract;

    public override bool Interact()
    {
        if (OnInteract == null) return false;

        OnInteract.Invoke();
        return true;
    }
}
