using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChangeUI : MonoBehaviour
{
    public void GoToUI(GameObject newUI)
    {
        newUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
