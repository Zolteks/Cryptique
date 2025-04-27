using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CinematicTextData", menuName = "Cryptique/CinematicTextData")]
public class CinematicTextData : ScriptableObject
{
    [Serializable]
    public class TextEntry
    {
        public float time; // Temps d'apparition
        public LocalizedString text; // Ton LocalizedString que tu as déjà
    }

    public List<TextEntry> entries = new();
}
