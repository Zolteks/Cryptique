using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonLauncher : OBJ_Interactable
{
    [SerializeField] private GameObject m_canva;

    GameObject m_UIPlay;

    public override bool Interact()
    {
        m_UIPlay = GameObject.Find("UIPlay");
        m_UIPlay.SetActive(false);

        PC_PlayerController.Instance.DisableInput();
        SGL_InteractManager.Instance.EnableInteraction();

        m_canva.SetActive(true);
        var pzl = GetComponent<PZL_Simon>();
        pzl.ResetPuzzle();
        pzl.ChangeCamState();
        GetComponent<BoxCollider>().enabled = false;
        return true;
    }
}
