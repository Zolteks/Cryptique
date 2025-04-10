using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IN_WellLauncher : OBJ_Interactable
{
    public override bool Interact()
    {
        Debug.Log("IN_WellLauncher Interact() called");
        Puzzle.StartPuzzle("WellUI");
        return true;
    }
}
