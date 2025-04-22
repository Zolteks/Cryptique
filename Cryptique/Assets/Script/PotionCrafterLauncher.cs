using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCrafterLauncher : OBJ_Interactable
{
    [SerializeField] private GameObject m_cameraObject;
    [SerializeField] private Camera m_camera;
    [SerializeField] private GameObject m_canva;
    public override bool Interact()
    {
        SGL_DragAndDrop.Instance.ChangeCamera(m_camera);
        SGL_InteractManager.Instance.ChangeCamera(m_camera);
        m_cameraObject.SetActive(true);
        m_canva.SetActive(true);
        GetComponent<BoxCollider>().enabled = false;
        return true;
    }
}
