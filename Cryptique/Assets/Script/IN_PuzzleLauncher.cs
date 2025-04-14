using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IN_PuzzleLauncher : OBJ_Interactable
{
    [SerializeField] private PuzzleData puzzleData;
    [SerializeField] private UnityEvent onSuccess;

    //private void OnMouseDown()
    //{
    //    if (false == CanInteract()) return;

    //    Interact();
    //}

    public override bool Interact()
    {
        Puzzle.StartPuzzle(puzzleData, onSuccess);

        return true;
    }
}
