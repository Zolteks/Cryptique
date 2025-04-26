using UnityEngine.Localization.Settings;
using System.Linq;

public class LangueSelector : OptionSelectorComponent<LanguageCode>
{
    private LanguageManager languageManager;

    protected override void Awake()
    {
        base.Awake();
        languageManager = LanguageManager.Instance;
    }

    protected override void LoadFromSave()
    {
        var value = saveSystemManager.GetGameData().settings.langue;

        if (options.Contains(value))
            currentIndex = options.IndexOf(value);
        else
            currentIndex = 0;
    }

    protected override void SaveToGameData(LanguageCode value)
    {
        saveSystemManager.GetGameData().settings.langue = value;
        var targetLocale = LocalizationSettings.AvailableLocales.Locales
            .FirstOrDefault(l => l.Identifier.Code == GetLocaleCode(value));

        if (targetLocale != null)
        {
            LocalizationSettings.SelectedLocale = targetLocale;
        }
        languageManager.RefreshAll(value);
    }

    private string GetLocaleCode(LanguageCode code)
    {
        return code switch
        {
            LanguageCode.EN => "en",
            LanguageCode.FR => "fr",
            _ => "en", // fallback
        };
    }
}
