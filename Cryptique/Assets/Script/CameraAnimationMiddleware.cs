using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimationMiddleware : MonoBehaviour
{
    [SerializeField] CameraRotator cam;

    public void SetIsBusy()
    {
        cam.SetIsBusy(false);
    }
}
