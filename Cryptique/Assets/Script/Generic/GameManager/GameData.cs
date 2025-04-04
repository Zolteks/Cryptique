using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public string currentTile;
    public string currentRegion;
    public int currentChapter;
    public string currentChapterName;

    public List<string> solvedPuzzles = new List<string>();
    public List<string> collectedItems = new List<string>();


    ////////////////////////////////// WIP
    public Vector4 cameraRotation;

    // Dialogues data here ?
}
