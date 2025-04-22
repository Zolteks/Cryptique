using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_DropOnHorse : OBJ_InteractOnDrop
{
    public override bool Interact()
    {
        GetComponent<PZL_Horse>().DoComplete();

        return true;
    }
}
