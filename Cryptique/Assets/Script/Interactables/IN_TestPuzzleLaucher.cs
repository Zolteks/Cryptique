using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IN_TestPuzzleLaucher : OBJ_Interactable
{
    [SerializeField] string puzzleName;
    public override bool Interact()
    {
        Puzzle.StartPuzzle(puzzleName);
        return true;
    }

    private void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

        Interact();
    }
}
