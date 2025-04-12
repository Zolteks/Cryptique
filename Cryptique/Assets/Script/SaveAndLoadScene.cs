using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveAndLoadScene : MonoBehaviour
{
    [SceneDropdown]
    public string sceneToLoad;

    public void Excute()
    {
        // Save the current scene
        SaveSystemManager.Instance.SaveGame();
        // Load the new scene
        //test si la scene existe 
        if (!Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            Debug.LogError($"Scene {sceneToLoad} does not exist.");
            return;
        }
        SceneManager.LoadScene(sceneToLoad);
    }

    public static void Excute(string sceneToLoad)
    {
        // Save the current scene
        SaveSystemManager.Instance.SaveGame();
        // Load the new scene
        //test si la scene existe 
        if (!Application.CanStreamedLevelBeLoaded(sceneToLoad))
        {
            Debug.LogError($"Scene {sceneToLoad} does not exist.");
            return;
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
