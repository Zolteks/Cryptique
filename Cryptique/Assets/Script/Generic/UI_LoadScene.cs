using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_LoadScene : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        // Load the specified scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}