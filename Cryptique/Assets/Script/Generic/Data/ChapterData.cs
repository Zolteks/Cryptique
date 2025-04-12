using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "ChapterData", menuName = "Cryptique/Chapter")]
public class ChapterData : ScriptableObject
{
    public string defaultChapterName;

    [SerializeField]
    private LocalizedString chapterName;

    public bool isUnlocked;
    public List<RegionData> regions;

    public string GetName()
    {
        return chapterName.GetLocalized(LanguageManager.Instance.GetCurrentLanguage(), defaultChapterName);
    }
}
