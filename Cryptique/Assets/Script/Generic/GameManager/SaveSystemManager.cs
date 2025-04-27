using SaveSystem;
using SaveSystem.SSJson;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

enum FrameRate
{
    Low = 30,
    Medium = 60,
    High = 90,
    Ultra = 120
}

public class SaveSystemManager : SingletonPersistent<SaveSystemManager>
{
    private SaveManager<GameDataJson> saveManager = new SaveManager<GameDataJson>();
    private string saveKey = "CryptiqueSaveData";
    private GameData gameData;
    [SerializeField] private FrameRate m_targetFrameRate = FrameRate.Medium;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = (int)m_targetFrameRate;
       
        Application.runInBackground = true;
        saveManager.Register(new JsonSaveSystem<GameDataJson>());
        gameData = new GameData();


        Test();
    }

    public void SaveGame()
    {
        var json = gameData.ToJson();
        saveManager.Save(saveKey, json);
        Debug.Log("Game saved.");
    }

    public void LoadGame()
    {
        if (saveManager.Exists(saveKey))
        {
            var result = saveManager.Load(saveKey);
            if (result.TryGet<JsonSaveSystem<GameDataJson>>(out var data))
            {
                gameData.ApplySave(data);
                Debug.Log("Game loaded.");

            } 
        }
        else
        {
            Debug.Log("No save file found. Initializing new game.");
        }
    }

    public void DeleteSave()
    {
        if (saveManager.Exists(saveKey))
        {
            saveManager.Delete(saveKey);
            Debug.Log("Save data deleted.");
        }
        else
        {
            Debug.LogError("No save data found to delete.");
        }
    }

    public GameData GetGameData()
    {
        if (gameData == null)
        {
            Debug.LogWarning("gameData was null. Creating default instance.");
            gameData = new GameData();
        }
        return gameData;
    }




#if UNITY_EDITOR
    public void OnApplicationQuit()
    {
        SaveGame();
    }
#else
    private bool hasSaved = false;

    public void OnApplicationFocus(bool focus)
    {
        if (!focus && !hasSaved)
        {
            SaveGame();
            hasSaved = true;
        }
        else if (focus)
        {
            hasSaved = false; // Reinitialise le flag quand l'app revient au premier plan
        }
    }

    public void OnApplicationPause(bool pause)
    {
        if (pause && !hasSaved)
        {
            SaveGame();
            hasSaved = true;
        }
        else if (!pause)
        {
            hasSaved = false; // Reinitialise si l'app revient de pause
        }
    }
#endif

    private void Test()
    {
        GameData gameData = new GameData();
        gameData.progression.IsTutorialDone = false;
        gameData.progression.currentRegion = "Tavern";
        gameData.progression.currentChapterName = "Wendigo";
        gameData.progression.completedPuzzles = new();
        gameData.progression.solvedPuzzles = new();
        gameData.progression.collectedItems = new();
        gameData.progression.unlockedChapters = new();
        gameData.progression.unlockedRegions = new();
        gameData.progression.completedPuzzles = new();

        gameData.settings.volumeMusic = 0.5f;
        gameData.settings.volumeSfx = 0.7f;
        gameData.settings.langue = LanguageCode.EN;
        gameData.settings.slideMode = SlideMode.Slide;

        // On assigne les donnees au save manager
        SaveSystemManager.Instance.GetGameData().ApplySave(gameData.ToJson());

        // Sauvegarde
        SaveSystemManager.Instance.SaveGame();
        Debug.Log("Donnees de test sauvegardees.");
    }
}
