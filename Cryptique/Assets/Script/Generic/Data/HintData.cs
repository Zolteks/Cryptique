using UnityEngine;


[CreateAssetMenu(fileName = "HintData", menuName = "Cryptique/Hint")]
public class HintData : ScriptableObject
{
    private bool isUnlocked;

    public string defaultHintText;
    [SerializeField]
    private LocalizedString hintText;

    public string GetText()
    {
        return hintText.GetLocalized(LanguageManager.Instance.GetCurrentLanguage(), defaultHintText);
    }

    public void SetUnlocked(bool unlocked)
    {
        isUnlocked = unlocked;
    }

    public bool IsUnlocked()
    {
        return isUnlocked;
    }

}

