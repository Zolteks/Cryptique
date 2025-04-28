using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PZL_Compass : Puzzle
{
    [SerializeField] private GameObject goCompass;
    [SerializeField] public Camera cam;
    private GameObject m_UIplayGameObject;
    private GameObject m_MapGameObject;

    void Start()
    {
        if (goCompass != null)
        {
            SGL_InteractManager.Instance.ChangeCamera(cam);

            PC_PlayerController.Instance.DisableInput();
            SGL_InteractManager.Instance.EnableInteraction();
            m_UIplayGameObject = GameObject.Find("UIPlay");
            m_MapGameObject = GameObject.Find("Map");

            StartCoroutine(CoroutineDestroyDetection());
        }
    }

    private void Update()
    {
        m_UIplayGameObject.SetActive(true);

        // Dsactier toute les composants Images ans les enfants de m_UIplayGameObject
        foreach (Image image in m_UIplayGameObject.GetComponentsInChildren<Image>())
        {
            image.enabled = false;
        }

        m_MapGameObject.SetActive(false);
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
        foreach (Image image in m_UIplayGameObject.GetComponentsInChildren<Image>())
        {
            image.enabled = true;
        }

        m_MapGameObject.SetActive(true);
    }

    void CompassDestroy()
    {
        PC_PlayerController.Instance.EnableInput();

        SGL_InteractManager.Instance.ChangeCamera(Camera.main);

        Complete();

        foreach (Image image in m_UIplayGameObject.GetComponentsInChildren<Image>())
        {
            image.enabled = true;
        }

        m_MapGameObject.SetActive(true);
    }
}
