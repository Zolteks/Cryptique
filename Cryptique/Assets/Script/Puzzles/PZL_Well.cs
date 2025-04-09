using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_Well : Puzzle
{

    [SerializeField] private GameObject lanterne;
    [SerializeField] private GameObject crank;


    public void Start()
    {
        //StartPuzzle(name);
        crank.SetActive(false);
    }

    public void AddCrank()
    {
        crank.SetActive(true);
    }

    public void PuzzleEnded()
    {
        Complete();
    }

}
