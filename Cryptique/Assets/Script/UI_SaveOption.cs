using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SaveOption : MonoBehaviour
{
    private SaveSystemManager saveSystemManager;

    private void Awake()
    {
        saveSystemManager = SaveSystemManager.Instance;
    }
}