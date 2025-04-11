using System;
using UnityEngine;

public class LangueSelector : OptionSelectorComponent
{
    LanguageManager languageManager;

    protected override void Awake()
    {
        base.Awake();
        languageManager = LanguageManager.Instance;
    }


    protected override void LoadFromSave()
    {
        string value = saveSystemManager.GetGameData().langue;
        if (options.Contains(value))
            currentIndex = options.IndexOf(value);
        else
            currentIndex = 0;
    }

    protected override void SaveToGameData(string value)
    {
        if(value == "Français")
            value = "FR";
        else if (value == "English")
            value = "EN";
        else
            value = "EN";
        saveSystemManager.GetGameData().langue = value;

        languageManager.RefreshAll();
    }
}
