using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MainMenu : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("InventoryTest", LoadSceneMode.Additive);
    }

    public void OptionGame()
    {
        UI_PreviousScene.Instance.SetPreviousScene("MainMenu");
        SceneManager.LoadScene("OptionsMenu", LoadSceneMode.Additive);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
