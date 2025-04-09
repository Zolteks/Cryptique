using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UI_StartMenuBook : MonoBehaviour
{
    public void OnApplicationQuit()
    {
        // Quit the application
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
