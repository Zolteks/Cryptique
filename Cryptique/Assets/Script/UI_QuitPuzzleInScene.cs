using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_QuitPuzzleInScene : MonoBehaviour
{
    [SerializeField] private GameObject m_backCam;
    [SerializeField] private BoxCollider m_collider;

    public void SwitchCamera()
    {
        gameObject.SetActive(false);
        m_backCam.SetActive(false);
        m_collider.enabled = true;
    }
}
