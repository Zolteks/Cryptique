using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_Compass : Puzzle
{
    [SerializeField] private GameObject goCompass;

    [SerializeField] public Camera cam;

    void Start()
    {
        if (goCompass != null)
        {
            SGL_InteractManager.Instance.ChangeCamera(cam);

            PC_PlayerController.Instance.DisableInput();
            SGL_InteractManager.Instance.EnableInteraction();

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
        PC_PlayerController.Instance.EnableInput();

        SGL_InteractManager.Instance.ChangeCamera(Camera.main);

        Complete();
    }
}
