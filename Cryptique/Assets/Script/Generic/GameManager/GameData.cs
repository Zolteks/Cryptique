using UnityEngine;
using System.Collections.Generic;

#region Enums

public enum LanguageCode { EN, FR }
public enum SlideMode { Arrow, Slide }
//public enum DaltonienMode { Default, Deuteranopia, Protanopia, Tritanopia }

#endregion

#region Serializable Data Containers

[System.Serializable]
public class PlayerProgressionData
{
    public string currentRegion;
    public string currentChapterName;
    public bool IsTutorialDone;
    public List<string> solvedPuzzles = new();
    public List<string> collectedItems = new();
    public List<float> cameraRotation = new();
    public List<string> unlockedChapters = new();
    public List<string> unlockedRegions = new();
    public List<string> completedPuzzles = new();

    public void SetCameraRotation(Vector4 rotation)
    {
        cameraRotation = new List<float> { rotation.x, rotation.y, rotation.z, rotation.w };
    }

    public Vector4 GetCameraRotation()
    {
        return new Vector4(
            cameraRotation.Count > 0 ? cameraRotation[0] : 0,
            cameraRotation.Count > 1 ? cameraRotation[1] : 0,
            cameraRotation.Count > 2 ? cameraRotation[2] : 0,
            cameraRotation.Count > 3 ? cameraRotation[3] : 0
        );
    }
}

[System.Serializable]
public class PlayerSettingsData
{
    public float volumeMusic = 1.0f;
    public float volumeSfx = 1.0f;
    public LanguageCode langue = LanguageCode.EN;
    //public DaltonienMode daltonienMode = DaltonienMode.Default;
    public SlideMode slideMode = SlideMode.Arrow;
}

[System.Serializable]
public class GameData
{
    public PlayerProgressionData progression = new();
    public PlayerSettingsData settings = new();

    [SerializeField] private List<ChapterData> chapters; // Runtime-only

    public void ApplySave(GameDataJson json)
    {
        progression = json.progression;
        settings = json.settings;
    }

    public GameDataJson ToJson()
    {
        return new GameDataJson
        {
            progression = progression,
            settings = settings
        };
    }
}

#endregion

#region JSON Wrapper

public class GameDataJson
{
    public PlayerProgressionData progression = new();
    public PlayerSettingsData settings = new();
}

#endregion

