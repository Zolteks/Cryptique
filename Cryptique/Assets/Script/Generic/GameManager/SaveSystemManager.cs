using SaveSystem;
using SaveSystem.SSJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystemManager : MonoBehaviour
{

    private SaveManager<GameData> saveManager = new SaveManager<GameData>();

    private string saveKey = "CryptiqueSaveData";

    void Awake()
    {
        Application.runInBackground = true;
        saveManager.Register(new JsonSaveSystem<GameData>());
    }


    private void Start()
    {
        TestSave();
        TestLoad();
    }

    public void SaveGame()
    {
        saveManager.Save(saveKey, new GameData());
    }

    public GameData LoadGame()
    {
        if (saveManager.Exists(saveKey))
        {
            var result = saveManager.Load(saveKey);

            if (result.TryGet<JsonSaveSystem<GameData>>(out var data))
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
        GameData data = new GameData();
        data.currentTile = "TestTile";
        data.currentRegion = "TestRegion";
        data.currentChapter = 1;
        data.currentChapterName = "TestChapter";
        data.solvedPuzzles.Add("Puzzle1");
        data.collectedItems.Add("Item1");
       //data.cameraRotation = new Vector4(0, 0, 0, 0);
        saveManager.Save(saveKey, data);
        Debug.Log("Test save completed.");
    }

    public void TestLoad()
    {
        GameData data = LoadGame();
        if (data != null)
        {
            Debug.Log("Test load completed.");
            Debug.Log($"Current Tile: {data.currentTile}");
            Debug.Log($"Current Region: {data.currentRegion}");
            Debug.Log($"Current Chapter: {data.currentChapter}");
            Debug.Log($"Current Chapter Name: {data.currentChapterName}");
            Debug.Log($"Solved Puzzles: {string.Join(", ", data.solvedPuzzles)}");
            Debug.Log($"Collected Items: {string.Join(", ", data.collectedItems)}");
           // Debug.Log($"Camera Rotation: {data.cameraRotation}");
        }
    }


}
