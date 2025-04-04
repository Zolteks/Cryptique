using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleB : Puzzle
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.B))
            Complete();
    }
}
