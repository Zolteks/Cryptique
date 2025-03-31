using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_Collectable : MonoBehaviour, IInteractable
{
    /* Variables */
    public bool bCanInteract { get; protected set; } = true;

    /* Functions */
    public bool Interact()
    {
        if (bCanInteract)
        {
            // Add to inventory
            return true;
        }
        return false;
    }

    public void UseOn()
    {
        // Use item on object
    }

    public void Drag()
    {
        
    }
}