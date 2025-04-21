using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpriteUITraduction", menuName = "Localization/SpriteUITraduction")]
public class SpriteUITraduction : ScriptableObject
{
    [System.Serializable]
    public class SpriteUIEntry
    {
        public string key;
        public Sprite frSprite;
        public Sprite enSprite;
    }
    public List<SpriteUIEntry> entries = new();
    private Dictionary<string, SpriteUIEntry> dict;
    public void Init()
    {
        dict = new Dictionary<string, SpriteUIEntry>();
        foreach (var entry in entries)
        {
            dict[entry.key] = entry;
        }
    }
    public Sprite GetSprite(string key, LanguageCode language)
    {
        if (dict == null || dict.Count == 0) Init();
        if (!dict.TryGetValue(key, out var entry))
            return null;
        return language switch
        {
            LanguageCode.FR => entry.frSprite,
            LanguageCode.EN => entry.enSprite,
            _ => entry.frSprite
        };
    }
}

