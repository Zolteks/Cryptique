using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TextUITraduction", menuName = "Localization/Text UI Traduction")]
public class TextUITraduction : ScriptableObject
{
    [System.Serializable]
    public class TextUIEntry
    {
        public string key;
        public string frText;
        public string enText;
    }

    public List<TextUIEntry> entries = new();

    private Dictionary<string, TextUIEntry> dict;

    public void Init()
    {
        dict = new Dictionary<string, TextUIEntry>();
        foreach (var entry in entries)
        {
            dict[entry.key] = entry;
        }
    }

    public string GetText(string key, LanguageCode language)
    {
        if (dict == null || dict.Count == 0) Init();

        if (!dict.TryGetValue(key, out var entry))
            return $"[{key}]";


        return language switch
        {
            LanguageCode.FR => entry.frText,
            LanguageCode.EN => entry.enText,
            _ => entry.frText
        };
    }
}
