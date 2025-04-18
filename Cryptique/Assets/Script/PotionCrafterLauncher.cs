using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionCrafterLauncher : OBJ_Interactable
{
    [SerializeField] private GameObject m_camera;
    [SerializeField] private GameObject m_canva;
    public override bool Interact()
    {
        m_camera.SetActive(true);
        m_canva.SetActive(true);
        GetComponent<BoxCollider>().enabled = false;
        return true;
    }
}
