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
        var value = saveSystemManager.GetGameData().langue;

        if (options.Contains(value))
            currentIndex = options.IndexOf(value);
        else
            currentIndex = 0;
    }

    protected override void SaveToGameData(LanguageCode value)
    {
        saveSystemManager.GetGameData().langue = value;
        languageManager.RefreshAll();
    }
}
