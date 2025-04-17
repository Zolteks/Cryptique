using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PZL_UndergroundLakeDoor : Puzzle
{
    [SerializeField] private PuzzleData puzzleData;
    [SerializeField] private UnityEvent onSuccess;

    public bool bSolved;

    private void Start()
    {
        bSolved = false;
    }

    public void Begin()
    {
        Puzzle.StartPuzzle(puzzleData, onSuccess);
    }

    public void PuzzleEnded()
    {
        Debug.Log("Puzzle ended");

        bSolved = true;

        Complete();
    }

}
