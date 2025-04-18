using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_ProgressionManager : MonoBehaviour
{   
    [SerializeField] private Slider chapterProgressBar;

    private GameProgressionManager gameProgressionManager;

    private void Awake()
    {
        gameProgressionManager = GameProgressionManager.Instance;
    }


    public void UpdatePuzzleProgress()
    {
        if (gameProgressionManager == null)
        {
            gameProgressionManager = GameProgressionManager.Instance;
        }

        int completedPuzzles = gameProgressionManager.GetCurrentRegion().GetCompletedPuzzlesCount();
        int totalPuzzles = gameProgressionManager.GetCurrentRegion().GetPuzzles().Count;
        int totalObjectCollected = gameProgressionManager.CollectedItemCount;
        int totalObject = gameProgressionManager.GetTotalItemsInRegion(gameProgressionManager.GetCurrentRegion().GetName());



        if (chapterProgressBar != null)
        {
            // Calculate the progress percentage
            // Object are a less value than puzzles
            float progress = (float)completedPuzzles / totalPuzzles;
            float objectProgress = (float)totalObjectCollected / totalObject;
            float totalProgress = (progress + objectProgress) / 2;

            // Update the progress bar
            chapterProgressBar.value = totalProgress;

            Debug.Log($"Progress: {progress * 100}%");
        }
    }
}
