using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OptionResetSave : MonoBehaviour
{

    //TODO: Implement this when save system is added
    public void ResetSave()
    {
        SaveSystemManager.Instance.DeleteSave();
    }
}
