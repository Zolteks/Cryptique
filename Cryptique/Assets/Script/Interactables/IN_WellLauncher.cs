using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IN_WellLauncher : OBJ_Interactable
{
    public override bool Interact()
    {
        Puzzle.StartPuzzle("PZL_Well");
        return true;
    }

    private void OnMouseDown()
    {
        Interact();
    }
}
