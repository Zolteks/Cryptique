using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideFromDirection : MonoBehaviour
{
    [SerializeField] bool top;
    [SerializeField] bool right;
    [SerializeField] bool bot;
    [SerializeField] bool left;

    private void Start()
    {
        GameManager.GetInstance().GetCamera().GetComponent<CameraRotator>().eDirectionUpdate += (CameraDirdection newDir) =>
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
