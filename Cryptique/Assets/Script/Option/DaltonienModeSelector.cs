public class DaltonienModeSelector : OptionSelectorComponent<string>
{
    protected override void LoadFromSave()
    {
        //string value = saveSystemManager.GetGameData().settings.daltonienMode;
        //if (options.Contains(value))
        //    currentIndex = options.IndexOf(value);
        //else
        //    currentIndex = 0;
    }

    protected override void SaveToGameData(string value)
    {
        //saveSystemManager.GetGameData().settings.daltonienMode = value;
    }
}
