using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_CrankFinding : Puzzle
{
    [SerializeField] private OBJ_Item m_Crank;
    [SerializeField] private GameObject m_StableIndoor;
    
    private GameObject m_StableDoor;
    private BoxCollider m_Collider;
    private GameObject m_UIplayGameObject;

    private void Start()
    {
        m_UIplayGameObject = GameObject.Find("UIPlay");
        m_UIplayGameObject.SetActive(false);

        m_StableDoor = GameObject.Find("EcurieOpen");
        m_Collider = m_StableDoor.GetComponent<BoxCollider>();

        PC_PlayerController.Instance.DisableInput();
        SGL_InteractManager.Instance.EnableInteraction();
    }

    public void QuitGame()
    {
        m_Collider.enabled = true;
        m_UIplayGameObject.SetActive(true);
        Quit();
    }

    public void TakeCrank()
    {
        SGL_InventoryManager.Instance.AddItem(m_Crank);
        Destroy(m_StableIndoor);

        m_UIplayGameObject.SetActive(true);
        PC_PlayerController.Instance.EnableInput();
        Complete();
    }
}
