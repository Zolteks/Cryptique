using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_GutterLabyrinth : Puzzle
{
    [SerializeField]PipeManager pipeManager;

    public void Solve()
    {
        if (pipeManager == null)
        {
            Debug.LogWarning("PipeManager.Instance est null !");
            return;
        }

        if (pipeManager.isSolved)
        {
            Debug.Log("Puzzle gutter solved !");
            Complete();
        }
    }
}
