using TMPro;
using UnityEngine;
using System.Collections.Generic;

public abstract class LocalizedOptionCycler : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI displayText;
    [SerializeField] protected List<string> optionKeys = new();
    [SerializeField] protected TextUITraduction localisationData;
    [SerializeField] protected string optionName;

    protected SaveSystemManager saveSystemManager;
    protected LanguageManager languageManager;

    protected int currentIndex = 0;

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

    protected virtual void NotifySelectedKey()
    {

    }

    //private void NotifySelectedKey()
    //{
    //    OptionChangeNotifier.Notify(optionName, optionKeys[currentIndex]);
    //    saveSystemManager.GetGameData().slideMode = optionKeys[currentIndex];
    //}
}
