using SaveSystem;
using SaveSystem.SSJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{

    private SaveManager<GameDataJson> saveManager = new SaveManager<GameDataJson>();

    private string saveKey = "CryptiqueSaveData";

    void Awake()
    {
        Application.runInBackground = true;
        saveManager.Register(new JsonSaveSystem<GameDataJson>());
    }


    private void Start()
    {
        TestSave();
        TestLoad();
    }

    public void SaveGame(GameDataJson gameDataJson)
    {
        saveManager.Save(saveKey, gameDataJson);
    }

    public GameDataJson LoadGame()
    {
        if (saveManager.Exists(saveKey))
        {
            var result = saveManager.Load(saveKey);

            if (result.TryGet<JsonSaveSystem<GameDataJson>>(out var data))
            {
                return data;
            }
        }
        return null;
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


    public void TestSave()
    {
        GameDataJson data = new GameDataJson();
        data.currentTile = "TestTile";
        data.currentRegion = "TestRegion";
        data.currentChapter = 1;
        data.currentChapterName = "TestChapter";
        data.solvedPuzzles.Add("Puzzle1");
        data.collectedItems.Add("Item1");
        data.cameraRotation = new List<int> { 0, 0, 0, 0 }; // Example rotation values
        saveManager.Save(saveKey, data);
        Debug.Log("Test save completed.");
    }

    public void TestLoad()
    {
        GameDataJson data = LoadGame();
        if (data != null)
        {
            Debug.Log("Test load completed.");
            Debug.Log($"Current Tile: {data.currentTile}");
            Debug.Log($"Current Region: {data.currentRegion}");
            Debug.Log($"Current Chapter: {data.currentChapter}");
            Debug.Log($"Current Chapter Name: {data.currentChapterName}");
            Debug.Log($"Solved Puzzles: {string.Join(", ", data.solvedPuzzles)}");
            Debug.Log($"Collected Items: {string.Join(", ", data.collectedItems)}");
            Debug.Log($"Camera Rotation: {string.Join(", ", data.cameraRotation)}");
        }
    }
}

public class GameDataJson
{
    public string currentTile;
    public string currentRegion;
    public int currentChapter;
    public string currentChapterName;
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
