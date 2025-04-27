using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PZL_Compass : Puzzle
{
    [SerializeField] private GameObject goCompass;
    [SerializeField] public Camera cam;

    GameObject m_UIPlay;

    void Start()
    {
        if (goCompass != null)
        {
            SGL_InteractManager.Instance.ChangeCamera(cam);

            PC_PlayerController.Instance.DisableInput();
            SGL_InteractManager.Instance.EnableInteraction();
            //m_UIPlay = GameObject.Find("UIPlay");
            //m_UIPlay.SetActive(false);

            StartCoroutine(CoroutineDestroyDetection());
        }
    }
    IEnumerator CoroutineDestroyDetection()
    {
        while (goCompass != null)
        {
            yield return null;
        }
        //m_UIPlay.SetActive(true);
        CompassDestroy();
    }

    public void QuitGame()
    {
        //m_UIPlay.SetActive(true);
        Quit();
    }

    void CompassDestroy()
    {
        PC_PlayerController.Instance.EnableInput();

        SGL_InteractManager.Instance.ChangeCamera(Camera.main);

        Complete();
    }
}
