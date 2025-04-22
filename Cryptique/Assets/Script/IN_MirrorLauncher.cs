using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Events;

public class IN_MirrorLauncher : OBJ_Interactable
{

    [SerializeField] private PuzzleData puzzleData;
    [SerializeField] private UnityEvent onSuccess;


    public override bool Interact()
    {
        Puzzle.StartPuzzle(puzzleData, onSuccess);

        gameObject.SetActive(false);

        return true;
    }
}
