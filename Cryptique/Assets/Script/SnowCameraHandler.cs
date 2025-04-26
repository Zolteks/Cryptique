using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowCameraHandler : MonoBehaviour
{
    [SerializeField] GameObject snowCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<CameraRotator>(out _))
        {
            snowCamera.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<CameraRotator>(out _))
        {
            snowCamera.SetActive(false);
        }
    }
}
