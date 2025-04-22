using System;
using System.Diagnostics;

class SliderLocalizedOptionCycler : LocalizedOptionCycler
{
    protected override void NotifySelectedKey()
    {
        OptionChangeNotifier.Notify(optionName, optionKeys[currentIndex]);
        if (Enum.IsDefined(typeof(SlideMode), currentIndex))
        {
            saveSystemManager.GetGameData().settings.slideMode = (SlideMode)currentIndex;
        }
        else
        {
            Debug.Print($"Index {currentIndex} n'est pas valide pour l'enum SlideMode.");
        }
    }
}

