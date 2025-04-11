using SaveSystem;
using SaveSystem.SSJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveSystemManager : SingletonPersistent<SaveSystemManager>
{
    private SaveManager<GameDataJson> saveManager = new SaveManager<GameDataJson>();

    private string saveKey = "CryptiqueSaveData";
    private GameData gameData;

    private void Start()
    {
        Application.runInBackground = true;
        saveManager.Register(new JsonSaveSystem<GameDataJson>());
        gameData = new GameData();

        TestSave();
    }

    public void SaveGame()
    {
        GameDataJson gameDataJson = new GameDataJson
        {
            currentTile = gameData.currentTile,
            currentRegion = gameData.currentRegion,
            currentChapter = gameData.currentChapter,
            currentChapterName = gameData.currentChapterName,
            IsTutorialDone = gameData.IsTutorialDone,
            solvedPuzzles = gameData.solvedPuzzles,
            collectedItems = gameData.collectedItems,
        };
        gameDataJson.setCameraRotation(gameData.cameraRotation);
        saveManager.Save(saveKey, gameDataJson);
    }

    public void LoadGame()
    {
        if (saveManager.Exists(saveKey))
        {
            var result = saveManager.Load(saveKey);

            if (result.TryGet<JsonSaveSystem<GameDataJson>>(out var data))
            {
                gameData.currentTile = data.currentTile;
                gameData.currentRegion = data.currentRegion;
                gameData.currentChapter = data.currentChapter;
                gameData.currentChapterName = data.currentChapterName;
                gameData.IsTutorialDone = data.IsTutorialDone;
                gameData.solvedPuzzles = data.solvedPuzzles;
                gameData.collectedItems = data.collectedItems;
                gameData.cameraRotation = data.getCameraRotation();
            }
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
        return gameData;
    }

    public void TestSave()
    {
        gameData.currentTile = "TestTile";
        gameData.currentRegion = "Tavern";
        gameData.currentChapter = 1;
        gameData.currentChapterName = "TestChapter";
        gameData.IsTutorialDone = true;
        gameData.solvedPuzzles.Add("Puzzle1");
        gameData.collectedItems.Add("Item1");
        gameData.cameraRotation = new Vector4(0, 0, 0, 0);
        SaveGame();
    }
}

public class GameDataJson
{
    public string currentTile;
    public string currentRegion;
    public int currentChapter;
    public string currentChapterName;
    public bool IsTutorialDone;
    public List<string> solvedPuzzles = new List<string>();
    public List<string> collectedItems = new List<string>();
    public List<int> cameraRotation = new List<int>();



    public void setCameraRotation(Vector4 rotation)
    {
        cameraRotation = new List<int> { (int)rotation.x, (int)rotation.y, (int)rotation.z, (int)rotation.w };
    }

    public Vector4 getCameraRotation()
    {
        return new Vector4(cameraRotation[0], cameraRotation[1], cameraRotation[2], cameraRotation[3]);
    }
}
