using System;
using System.Collections;
using System.Collections.Generic;
using DanielLochner.Assets.SimpleScrollSnap;
using UnityEngine;
using UnityEngine.UI;

public class PZL_Lock : Puzzle
{
    [SerializeField] List<int> code;
    [SerializeField] Camera cam;
    [SerializeField]  List<SimpleScrollSnap> scrollAreas;

    private GameObject m_LockerArmory;
    private BoxCollider m_Collider;

    private PC_PlayerController m_playerController;
    private void Start()
    {
        m_playerController = PC_PlayerController.Instance;
        SGL_InteractManager.Instance.ChangeCamera(cam);
        m_playerController.DisableInput();

        m_LockerArmory = GameObject.Find("Armoire_lock");
        m_Collider = m_LockerArmory.GetComponent<BoxCollider>();
    }

    public void UpdateCode()
    {
        for (int i = 0; i < code.Count; i++)
        {
            var panelID = scrollAreas[i].GetNearestPanel();
            var correctPanel = scrollAreas[i].transform.GetChild(0).GetChild(0).GetChild(panelID);
            var panelTxt = correctPanel.GetChild(0).GetComponent<Text>().text;

            int value = 0;
            if(Int32.TryParse(panelTxt, out value))
            {
                if (value != code[i])
                {
                    print("incorrect code");
                    return;
                }
            }
            else
            {
                Debug.LogError("A value from the lock is not correct! : "+ panelTxt);
                return;
            }
        }

        m_Collider.enabled = false;
        m_playerController.EnableInput();
        SGL_InteractManager.Instance.ChangeCamera(Camera.main);
        Complete();
    }
}
