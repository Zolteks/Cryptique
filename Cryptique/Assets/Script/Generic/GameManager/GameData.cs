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
    public LanguageCode langue = LanguageCode.EN;

    // Daltonien
    public string daltonienMode = "Default";

    // Slide
    public string slideMode = "Arrow"; // arrow, slide



    [SerializeField] private List<ChapterData> chapters;

    public List<string> GetAllRegions(string chapterName)
    {
        foreach (var chapter in chapters)
        {
            if (chapter.GetName() == chapterName)
            {
                return chapter.regions.ConvertAll(region => region.GetName());
            }
        }
        Debug.LogWarning($"Chapter '{chapterName}' not found.");
        return new List<string>();
    }

    public List<string> GetAllPuzzles(string regionName)
    {
        foreach (var item in regionName)
        {
            var region = chapters.Find(c => c.GetName() == currentChapterName)
                .regions.Find(r => r.GetName() == regionName);
            if (region != null)
            {
                return region.GetPuzzles().ConvertAll(puzzle => puzzle.GetPuzzleID());
            }
            else
            {
                Debug.LogWarning($"Region '{regionName}' not found.");
                return new List<string>();
            }
        }

        return new List<string>();
    }

    public string GetPuzzleDescription(string puzzleID)
    {
        foreach (var chapter in chapters)
        {
            foreach (var region in chapter.regions)
            {
                var puzzle = region.GetPuzzles().Find(p => p.GetPuzzleID() == puzzleID);
                if (puzzle != null)
                {
                    return puzzle.GetDescription();
                }
            }
        }
        Debug.LogWarning($"Puzzle '{puzzleID}' not found.");
        return string.Empty;
    }
}
public enum LanguageCode
{
    EN,
    FR,
}
