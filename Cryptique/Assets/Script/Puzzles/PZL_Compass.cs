using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_Compass : Puzzle
{
    [SerializeField] private GameObject goCompass;

    void Start()
    {
        if (goCompass != null)
        {
            StartCoroutine(CoroutineDestroyDetection());
        }
    }
    IEnumerator CoroutineDestroyDetection()
    {
        while (goCompass != null)
        {
            yield return null;
        }
        CompassDestroy();
    }

    void CompassDestroy()
    {
        Complete();
    }
}
