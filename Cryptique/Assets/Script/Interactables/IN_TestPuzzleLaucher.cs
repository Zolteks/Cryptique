using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IN_TestPuzzleLaucher : OBJ_Interactable
{
    public override bool Interact()
    {
        Puzzle.StartPuzzle("TestPuzzle");
        return true;
    }

    private void OnMouseDown()
    {
        Interact();
    }
}
