using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PZL_Lantern : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private PZL_Crank crank;
    [SerializeField] private PZL_Well well;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (crank.pickAble())
        {
            well.PuzzleEnded();
        }

    }
}
