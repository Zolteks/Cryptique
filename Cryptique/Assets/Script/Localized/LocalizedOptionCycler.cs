using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class LocalizedOptionCycler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayText;
    [SerializeField] private List<string> optionKeys = new();
    [SerializeField] private TextUITraduction localisationData;

    SaveSystemManager saveSystemManager;
    LanguageManager languageManager;

    private int currentIndex = 0;

    private void Awake()
    {
        saveSystemManager = SaveSystemManager.Instance;
        languageManager = LanguageManager.Instance;

        languageManager.Register(OnLanguageChanged);
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void OnDestroy()
    {
        languageManager.Unregister(OnLanguageChanged);
    }

    public void Next()
    {
        currentIndex = (currentIndex + 1) % optionKeys.Count;
        UpdateDisplay();
        NotifySelectedKey();
    }

    public void Previous()
    {
        currentIndex = (currentIndex - 1 + optionKeys.Count) % optionKeys.Count;
        UpdateDisplay();
        NotifySelectedKey();
    }

    private void UpdateDisplay()
    {
        if (displayText != null && optionKeys.Count > 0)
        {
            string key = optionKeys[currentIndex];
            if (localisationData == null) return;
            displayText.text = localisationData.GetText(key, languageManager.GetCurrentLanguage());
        }
    }

    private void OnLanguageChanged(LanguageCode _)
    {
        UpdateDisplay();
    }

    private void NotifySelectedKey()
    {
        OptionChangeNotifier.Notify("slideMode", optionKeys[currentIndex]);
        saveSystemManager.GetGameData().slideMode = optionKeys[currentIndex];
    }
}
