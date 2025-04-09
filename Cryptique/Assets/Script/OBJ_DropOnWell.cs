using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_DropOnWell : OBJ_InteractOnDrop
{
    public override bool Interact()
    {
        var component = GetComponent<IN_WellLauncher>();
        if (component != null)
        {
            component.enabled = true;
            return true;
        }
        else
        {
            Debug.LogError("IN_WellLauncher component not found in children of " + gameObject.name);
            return false;
        }
    }
}
