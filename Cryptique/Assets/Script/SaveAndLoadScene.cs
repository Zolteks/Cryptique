using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoadScene : MonoBehaviour
{
    public static void Excute(string sceneName)
    {
        // Save the current scene
        SaveSystemManager.Instance.SaveGame();
        // Load the new scene
        SceneManager.LoadScene(sceneName);
    }
}
