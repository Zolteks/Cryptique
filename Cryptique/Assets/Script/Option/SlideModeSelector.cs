using System.Diagnostics;

public class SlideModeSelector : OptionSelectorComponent
{
    protected override void LoadFromSave()
    {
        string value = saveSystemManager.GetGameData().slideMode;
        if (options.Contains(value))
            currentIndex = options.IndexOf(value);
        else
            currentIndex = 0;
    }

    protected override void SaveToGameData(string value)
    {
        saveSystemManager.GetGameData().slideMode = value;
    }
}
