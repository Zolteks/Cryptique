using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PZL_Lantern : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PZL_Crank crank;
    [SerializeField] private PZL_Well well;
    [SerializeField] private OBJ_Item m_lantern;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (crank.pickAble())
        {
            //Add the lantern to the inventory logic here !
            SGL_InventoryManager.Instance.AddItem(m_lantern);
            well.PuzzleEnded();
        }

    }

}
