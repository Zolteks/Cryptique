using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RegionData", menuName = "Cryptique/Region")]
public class RegionData : ScriptableObject
{
    public string defaultRegionName;
    [SerializeField]
    private LocalizedString regionName;

    public bool isUnlocked;
    public int totalItems;
    [SerializeField]
    private List<PuzzleData> puzzles;
    [SerializeField]
    private string sceneName;

    [SerializeField]
    private AudioClip backgroundMusic;

    public string GetName()
    {
        return regionName.GetLocalized(LanguageManager.Instance.GetCurrentLanguage(), defaultRegionName);
    }

    public List<PuzzleData> GetPuzzles()
    {
        return puzzles;
    }

    public int GetCompletedPuzzlesCount()
    {
        int count = 0;
        foreach (var puzzle in puzzles)
        {
            if (puzzle.IsCompleted())
            {
                count++;
            }
        }
        return count;
    }

    public string GetSceneName()
    {
        return sceneName;
    }

    public AudioClip GetBackgroundMusic()
    {
        return backgroundMusic;
    }

    public void SetBackgroundMusic(AudioClip audioClip)
    {
        backgroundMusic = audioClip;
    }
}
