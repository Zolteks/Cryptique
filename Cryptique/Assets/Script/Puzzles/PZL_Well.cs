using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_Well : Puzzle
{

    [SerializeField] private GameObject lanterne;
    [SerializeField] private GameObject crank;
    
    private void Update()
    {
        if (lanterne == null)
        {
            Complete();
        }
    }

}
