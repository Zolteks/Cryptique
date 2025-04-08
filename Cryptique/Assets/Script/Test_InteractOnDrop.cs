using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_InteractOnDrop : OBJ_InteractOnDrop
{
    public override bool Interact()
    {
        Destroy(gameObject);
        return true;
    }
}
