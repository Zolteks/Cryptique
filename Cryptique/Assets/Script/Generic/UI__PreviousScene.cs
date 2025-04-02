using UnityEngine;

public class UI_PreviousScene : MonoBehaviour
{
    private static UI_PreviousScene instance;
    private string sPreviousSceneName;

    public static UI_PreviousScene Instance
    {
        get { return instance; }
        private set { instance = value; }
    }

    public string GetPreviousScene()
    {
        return sPreviousSceneName;
    }

    private void Awake()
    {
        Instance = this;
    }

    public void SetPreviousScene(string sceneName)
    {
        sPreviousSceneName = sceneName;
    }
}
