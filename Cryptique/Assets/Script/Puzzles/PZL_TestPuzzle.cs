using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_TestPuzzle : Puzzle
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            Complete();
    }
}
