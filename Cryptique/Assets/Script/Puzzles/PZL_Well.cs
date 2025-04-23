using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_Well : Puzzle
{

    [SerializeField] private GameObject lanterne;
    [SerializeField] private GameObject crank;
    [SerializeField] public Camera cam;

    private GameObject m_UIplayGameObject;

    private void Start()
    {
        m_UIplayGameObject = GameObject.Find("UIPlay");
        m_UIplayGameObject.SetActive(false);
        SGL_InteractManager.Instance.ChangeCamera(cam);
    }

    private void Update()
    {
        if (lanterne == null)
        {
            SGL_InteractManager.Instance.ChangeCamera(Camera.main);
            m_UIplayGameObject.SetActive(true);
            Complete();
        }
    }

}
