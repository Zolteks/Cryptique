using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IN_Generic : OBJ_Interactable
{
    [SerializeField] UnityEvent OnInteract;

    override public bool Interact()
    {
        OnInteract?.Invoke();

        return true;
    }

    //private void OnMouseDown()
    //{
    //    if (false == CanInteract()) return;

    //    Interact();
    //}
}
