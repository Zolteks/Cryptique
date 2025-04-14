using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IN_WellLauncher : OBJ_InteractOnDrop
{
    [SerializeField] private PZL_Well m_Well;

    public override bool Interact()
    {
        //Puzzle.StartPuzzle("PZL_Well");
        m_Well.AddCrank();
        Destroy(gameObject);
        return true;
    }

    private void OnMouseDown()
    {
        Interact();
    }
}
