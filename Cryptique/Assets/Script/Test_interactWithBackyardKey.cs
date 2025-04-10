using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractWithBackyardKey : OBJ_InteractOnDrop
{
    [SerializeField] private UnityEvent onSuccess;
    [SerializeField] private  Animation animationOpen; // Animation of the player climbing the ladder CAREFULL (maybe not Animation)
    public override bool Interact()
    {
        onSuccess?.Invoke();
        return true;
    }

}
