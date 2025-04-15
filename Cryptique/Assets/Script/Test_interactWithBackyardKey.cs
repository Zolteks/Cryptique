using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractWithBackyardKey : OBJ_InteractOnDrop
{
    public override bool Interact()
    {
        GetComponent<PZL_Gate>().DoComplete();
        return true;
    }

}
