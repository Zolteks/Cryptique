using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public string currentTile;
    public string currentRegion;
    public int currentChapter;
    public string currentChapterName;

    //Bool for change UI start
    public bool IsTutorialDone;

    public List<string> solvedPuzzles = new List<string>();
    public List<string> collectedItems = new List<string>();

    ////////////////////////////////// WIP
    public Vector4 cameraRotation;

    // Option data
    // Audio
    public float volumeMusic = 1.0f;
    public float volumeSfx = 1.0f;

    // Language
    public string langue = "EN";

    // Daltonien
    public string daltonienMode = "Default";

    // Slide
    public string slideMode = "Arrow"; // arrow, slide
}
