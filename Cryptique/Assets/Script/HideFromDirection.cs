using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideFromDirection : MonoBehaviour
{
    [SerializeField] bool top;
    [SerializeField] bool right;
    [SerializeField] bool bot;
    [SerializeField] bool left;

    // TODO: To be replaced by and access trough game manager singleton or similar
    [SerializeField] CameraRotator m_camera;

    private void Start()
    {
        m_camera.eDirectionUpdate += (CameraDirdection newDir) =>
        {
            if(newDir == CameraDirdection.top)
            {
                gameObject.SetActive(!top);
            }
            else if(newDir == CameraDirdection.right)
            {
                gameObject.SetActive(!right);
            }
            else if(newDir == CameraDirdection.left)
            {
                gameObject.SetActive(!left);
            }
            else if (newDir == CameraDirdection.bot)
            {
                gameObject.SetActive(!bot);
            }
        };
    }
}
