public class DaltonienModeSelector : OptionSelectorComponent
{
    protected override void LoadFromSave()
    {
        string value = saveSystemManager.GetGameData().daltonienMode;
        if (options.Contains(value))
            currentIndex = options.IndexOf(value);
        else
            currentIndex = 0;
    }

    protected override void SaveToGameData(string value)
    {
        saveSystemManager.GetGameData().daltonienMode = value;
    }
}
