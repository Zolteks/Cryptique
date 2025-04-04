using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLZ_TestPuzzle : Puzzle
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            Complete();
    }
}
