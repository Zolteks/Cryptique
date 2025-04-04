using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ChangeUI : MonoBehaviour
{
    public void GoToUI(GameObject newUI)
    {
        // Open the chapter
        Debug.Log("Opening new UI: " + newUI.name);
        newUI.SetActive(true);
        gameObject.SetActive(false);
    }
}
