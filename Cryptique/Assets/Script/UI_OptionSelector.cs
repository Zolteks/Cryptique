using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_OptionSelector : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI OptionText;
    [SerializeField] private List<string> Options;

    private SaveSystemManager saveSystemManager;

    private void Awake()
    {
        saveSystemManager = SaveSystemManager.Instance;
    }

    public void SetNextOption()
    {
        if (!ErrorCheck()) return;

        int currentIndex = Options.IndexOf(OptionText.text);
        currentIndex++;
        if (currentIndex >= Options.Count)
        {
            currentIndex = 0;
        }
        OptionText.text = Options[currentIndex];

    }

    public void SetPreviousOption()
    {   
        if(!ErrorCheck()) return;

        int currentIndex = Options.IndexOf(OptionText.text);
        currentIndex--;
        if (currentIndex >= Options.Count)
        {
            currentIndex = Options.Count - 1;
        }
        OptionText.text = Options[currentIndex];
    }

    private bool ErrorCheck()
    {
        if (Options.Count == 0)
        {
            Debug.LogError("No options set for UI_OptionSelector");
            return false;
        }
        return true;
    }
}
