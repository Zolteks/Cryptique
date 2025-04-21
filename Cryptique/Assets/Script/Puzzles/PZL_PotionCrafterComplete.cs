using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_PotionCrafterComplete : Puzzle
{
    public void PotionComplete()
    {
        SGL_DragAndDrop.Instance.ChangeCamera(Camera.main);
        SGL_InteractManager.Instance.ChangeCamera(Camera.main);
        Complete();
    }
}
