using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleD : Puzzle
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
            Complete();
    }
}
