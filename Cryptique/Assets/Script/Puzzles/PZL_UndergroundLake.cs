using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_UndergroundLakeDoor : Puzzle
{

    public bool bSolved;

    private void Start()
    {
        bSolved = false;
    }

    public void Begin()
    {
        Puzzle.StartPuzzle("Mirror");
    }

    public void PuzzleEnded()
    {
        Debug.Log("Puzzle ended");

        bSolved = true;

        Complete();
    }

}
