using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PuzzleData", menuName = "Cryptique/Puzzle")]
public class PuzzleData : ScriptableObject
{
    public string defaultPuzzleID;
    public string defaultDescription;

    private bool isCompleted = false;
    private bool isUnlocked = false;

    [SerializeField]
    private LocalizedString puzzleID;
    [SerializeField]
    private LocalizedString description;

    [SerializeField]
    private LocalizedString puzzleDescription;

    [SerializeField]
    private List<PuzzleData> prerequisites;

    [SerializeField]
    private List<HintData> hints;

    public string GetPuzzleID()
    {
        return puzzleID.GetLocalized(LanguageManager.Instance.GetCurrentLanguage(), defaultPuzzleID);
    }

    public string GetDescription()
    {
        return description.GetLocalized(LanguageManager.Instance.GetCurrentLanguage(), defaultDescription);
    }

    public string GetPuzzleDescription()
    {
        return puzzleDescription.GetLocalized(LanguageManager.Instance.GetCurrentLanguage(), defaultDescription);
    }

    public List<PuzzleData> GetPrerequisites()
    {
        return prerequisites;
    }

    public bool IsCompleted()
    {
        return isCompleted;
    }

    public void SetCompleted(bool completed)
    {
        isCompleted = completed;
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }

    public void SetUnlocked(bool unlocked)
    {
        isUnlocked = unlocked;
    }

    public List<HintData> GetHints()
    {
        return hints;
    }
}
