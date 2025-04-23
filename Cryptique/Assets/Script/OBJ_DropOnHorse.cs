using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_DropOnHorse : OBJ_InteractOnDrop
{
    private int counterOBj = 0;
    public override bool Interact()
    {
        counterOBj++;

        if (counterOBj == 2) GetComponent<PZL_Horse>().DoComplete();

        return true;
    }
}
