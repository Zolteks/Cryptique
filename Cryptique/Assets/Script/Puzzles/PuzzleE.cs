using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleE : PuzzleD
{
    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
            Complete();
    }
}
