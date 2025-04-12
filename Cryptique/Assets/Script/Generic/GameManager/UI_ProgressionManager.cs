using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_ProgressionManager : MonoBehaviour
{
    /* Variables */
    [SerializeField] private TextMeshProUGUI regionUnlockedUI;
    [SerializeField] private TextMeshProUGUI itemProgressUI;
    [SerializeField] private TextMeshProUGUI completedPuzzlesUI;
    [SerializeField] private List<TextMeshProUGUI> puzzleDescriptionTexts;
    
    private TextMeshProUGUI itemCountText;
    private TextMeshProUGUI itemRegionText;

    [SerializeField] private Slider chapterProgressBar;

    private GameProgressionManager gameProgressionManager;

    private void Awake()
    {
        gameProgressionManager = GameProgressionManager.Instance;
    }

    /* Getters and Setters */
    
    public void UpdateItemProgress(string itemRegion, int collectedItems, int totalItems)
    {
        if (itemProgressUI != null)
        {
            itemProgressUI.text = $"{itemRegion} Items : {collectedItems}/{totalItems}";
            Debug.Log($"Item Progress UI Updated: {itemRegion}: {collectedItems}/{totalItems}");
        }
    }

    public void UpdateRegionUnlocked(string itemRegion)
    {
        if (regionUnlockedUI != null)
        {
            regionUnlockedUI.text = $"{itemRegion}";
            Debug.Log($"Region Unlocked UI Updated: {itemRegion}");
        }
    }

    public void UpdatePuzzleProgress()
    {
        if (gameProgressionManager == null) return;


        int completedPuzzles = gameProgressionManager.GetCurrentRegion().GetCompletedPuzzlesCount();
        var totalPuzzles = gameProgressionManager.GetCurrentRegion().GetPuzzles().Count;

        if (completedPuzzlesUI != null)
        {
            completedPuzzlesUI.text = $"{completedPuzzles}/{totalPuzzles}";
        }

        if (chapterProgressBar != null)
        {
            chapterProgressBar.value = totalPuzzles > 0 ? (float)completedPuzzles / totalPuzzles : 0;
        }

        Debug.Log("Puzzle UI updated");
    }

    public void UpdatePuzzleDescriptions(List<string> descriptions)
    {
        for (int i = 0; i < puzzleDescriptionTexts.Count; i++)
        {
            if (i < descriptions.Count)
            {
                puzzleDescriptionTexts[i].text = descriptions[i];
            }
            else
            {
                puzzleDescriptionTexts[i].text = "";
            }
        }
    }
}
