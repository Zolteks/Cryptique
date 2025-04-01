using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class OBJ_Interactable : MonoBehaviour, IInteractable
{
    /* Variables */
    public bool bCanInteract { get; protected set; } = true;

    /* Functions */
    public abstract bool Interact();
}
