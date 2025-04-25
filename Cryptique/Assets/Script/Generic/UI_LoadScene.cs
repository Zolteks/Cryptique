using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_LoadScene : MonoBehaviour
{
    [SerializeField]
    public string sceneToLoad;


    public void LoadScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
