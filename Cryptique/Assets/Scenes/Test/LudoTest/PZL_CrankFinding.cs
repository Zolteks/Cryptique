using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_CrankFinding : MonoBehaviour
{
    [SerializeField] private OBJ_Item m_Crank;
    [SerializeField] private GameObject m_StableIndoor;

    public void TakeCrank()
    {
        SGL_InventoryManager.Instance.AddItem(m_Crank);
        Destroy(m_StableIndoor);
    }
}
