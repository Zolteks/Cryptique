using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class HideFromDirection : MonoBehaviour
{
    [SerializeField] bool top;
    [SerializeField] bool right;
    [SerializeField] bool bot;
    [SerializeField] bool left;

    private void Awake()
    {
        var cam = GameManager.GetInstance().GetCamera().GetComponent<CameraRotator>();
        cam.eDirectionUpdate += (CameraDirdection newDir) =>
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

        //switch (cam.GetDirection())
        //{
        //    case CameraDirdection.top:
        //        if (top)
        //            gameObject.SetActive(false);
        //        break;
        //    case CameraDirdection.right:
        //        if (right)
        //            gameObject.SetActive(false);
        //        break;
        //    case CameraDirdection.left:
        //        if (left)
        //            gameObject.SetActive(false);
        //        break;
        //    case CameraDirdection.bot:
        //        if (bot)
        //            gameObject.SetActive(false);
        //        break;
        //}
    }
}


