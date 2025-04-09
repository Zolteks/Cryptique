using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LoadScene : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        // Load the specified scene
        SceneManager.LoadScene(sceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}