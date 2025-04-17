using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_ChainComplete : Puzzle
{
    [SerializeField] private GameObject m_FindCrank;

    public void CompleteChainPuzzle()
    {
        Instantiate(m_FindCrank);
        Complete();
    }
}
