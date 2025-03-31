using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    bool bCanInteract { get; }
    bool Interact();
}
