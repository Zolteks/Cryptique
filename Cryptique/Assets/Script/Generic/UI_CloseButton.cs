using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_CloseButton : MonoBehaviour
{
    public void ClosePanel()
    {
        string sPreviousScene = UI_PreviousScene.Instance.GetPreviousScene();
        SceneManager.LoadScene(sPreviousScene, LoadSceneMode.Single);
    }
}

