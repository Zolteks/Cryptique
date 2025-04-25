using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLocker : MonoBehaviour
{
    [SerializeField] bool top;
    [SerializeField] bool right;
    [SerializeField] bool bot;
    [SerializeField] bool left;

    [Tooltip("The side the camera will be watching at arrival")]
    [SerializeField] CameraDirdection facingSide = CameraDirdection.top;



    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<CameraRotator>(out CameraRotator cam))
        {
            Dictionary<CameraDirdection, bool> newAuthorizations = new()
            {
                {CameraDirdection.bot, bot },
                {CameraDirdection.right, right },
                {CameraDirdection.top, top },
                {CameraDirdection.left, left },
            };
            cam.SetAllowedRotation(newAuthorizations);

            cam.ForceOrientation(facingSide);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<CameraRotator>(out CameraRotator cam))
        {
            cam.ResetAllowedDirections();
        }
    }

}
