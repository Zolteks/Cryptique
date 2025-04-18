using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_Well : Puzzle
{

    [SerializeField] private GameObject lanterne;
    [SerializeField] private GameObject crank;
    [SerializeField] public Camera cam;

    private void Start()
    {
        SGL_InteractManager.Instance.ChangeCamera(cam);
    }

    private void Update()
    {
        if (lanterne == null)
        {
            SGL_InteractManager.Instance.ChangeCamera(Camera.main);
            Complete();
        }
    }

}
