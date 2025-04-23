using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_CrankFinding : Puzzle
{
    [SerializeField] private OBJ_Item m_Crank;
    [SerializeField] private GameObject m_StableIndoor;

    private GameObject m_UIplayGameObject;

    private void Start()
    {
        m_UIplayGameObject = GameObject.Find("UIPlay");
        m_UIplayGameObject.SetActive(false);
    }

    public void TakeCrank()
    {
        SGL_InventoryManager.Instance.AddItem(m_Crank);
        Destroy(m_StableIndoor);

        m_UIplayGameObject.SetActive(true);
        Complete();
    }
}
