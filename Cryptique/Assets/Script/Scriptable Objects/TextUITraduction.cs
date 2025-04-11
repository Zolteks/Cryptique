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

    public string GetText(string key, string language)
    {
        if (dict == null || dict.Count == 0) Init();

        if (!dict.TryGetValue(key, out var entry))
            return $"[{key}]";

        //Test available language
        if (string.IsNullOrEmpty(language))
            throw new System.Exception($"Language is null or empty for key: {key}");
        if (language != "FR" && language != "EN")
            throw new System.Exception($"Language is not supported for key: {key}");


        return language switch
        {
             "FR"=> entry.frText,
            "Français" => entry.frText,
            "EN" => entry.enText,
            "English" => entry.enText,
            _ => entry.enText
        };
    }
}
