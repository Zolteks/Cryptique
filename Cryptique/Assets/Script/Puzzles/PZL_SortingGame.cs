using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PZL_SortingGame : Puzzle
{
    private List<bool> etatsPlacement;
    private bool bIsAllPlaced = false;
    [SerializeField] private int iElementNumber;

    void Start()
    {
        etatsPlacement = new List<bool>(new bool[iElementNumber]);
    }

    public void UpdateEtatPlacement(int index, bool estBienPlace)
    {
        if (index >= 0 && index < etatsPlacement.Count)
        {
            etatsPlacement[index] = estBienPlace;
            CheckPlacementComplet();
        }
    }

    private void CheckPlacementComplet()
    {
        if (etatsPlacement.All(etat => etat))
        {
            bIsAllPlaced = true;
            Complete();
        }
    }

    public bool GetisAllPlaced()
    {
        return bIsAllPlaced;
    }
}
