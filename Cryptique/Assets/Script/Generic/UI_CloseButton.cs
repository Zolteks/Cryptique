using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_CloseButton : MonoBehaviour
{
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}

