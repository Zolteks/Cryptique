using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChangeUI : MonoBehaviour
{
    public void GoToUI(GameObject newUI)
    {
        newUI.SetActive(true);
        gameObject.SetActive(false);

        //LOG
        Debug.Log($"UI {gameObject.name} is now inactive");
        Debug.Log($"UI {newUI.name} is now active");
    }
}
