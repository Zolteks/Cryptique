using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    private void Awake()
    {
        // On lance le chargement async
        StartCoroutine(InitBootstrap());
    }

    private IEnumerator InitBootstrap()
    {
        yield return new WaitUntil(() => SaveSystemManager.Instance != null);

        SaveSystemManager.Instance.LoadGame();

        Debug.Log("Bootstrap : Game loaded");

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