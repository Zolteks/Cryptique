using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestAccess : MonoBehaviour
{
    [SerializeField] int amountOfConds = 3;
    int currentCount = 0;

    public void UpdateCond()
    {
        currentCount++;
        if(currentCount == amountOfConds)
        {
            gameObject.SetActive(true);
        }
    }
}
