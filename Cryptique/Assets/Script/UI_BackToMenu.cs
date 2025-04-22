using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_BackToMenu : MonoBehaviour
{
    public void BackToMenu()
    {
        if (SaveSystemManager.Instance.GetGameData().progression.IsTutorialDone)
        {
            SceneManager.LoadScene("MainMenuGrimoire", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
        }
    }
}
