using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LocalizedString
{
    [Serializable]
    public class LocalizedEntry
    {
        public LanguageCode languageCode;
        [TextArea]
        public string value;
    }

    [SerializeField]
    private List<LocalizedEntry> entries = new();

    public string GetLocalized(LanguageCode lang, string fallback = "")
    {
        var match = entries.Find(e => e.languageCode == lang);
        return match != null ? match.value : fallback;
    }
}
