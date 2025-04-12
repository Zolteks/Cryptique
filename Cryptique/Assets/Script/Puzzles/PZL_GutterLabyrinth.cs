using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_GutterLabyrinth : Puzzle
{
    PipeManager pipeManager;
    private void Update()
    {
        if (PipeManager.CheckVictory())
        {
            Complete();
        }
    }
}
