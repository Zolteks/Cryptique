using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    /* Variables */
    [SerializeField] private TextMeshProUGUI regionUnlockedUI;
    [SerializeField] private TextMeshProUGUI itemProgressUI;
    [SerializeField] private TextMeshProUGUI currentPuzzlesUI;
    [SerializeField] private TextMeshProUGUI completedPuzzlesUI;
    [SerializeField] private TextMeshProUGUI puzzleDescriptionText;
    [SerializeField] private Slider chapterProgressBar;

    /* Getters and Setters */
    private int GetTotalPuzzles()
    {
        var progressionManager = GameProgressionManager.GetInstance();
        int total = 0;
        foreach (var region in progressionManager.GetRegions())
        {
            total += progressionManager.GetTotalItemsInRegion(region); // Remplace by a real total puzzle
        }
        return total;
    }

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
        var progressionManager = GameProgressionManager.GetInstance();
        if (progressionManager == null) return;


        var completedPuzzles = progressionManager.GetCompletedPuzzles();
        var totalPuzzles = GetTotalPuzzles();

        if (completedPuzzlesUI != null)
        {
            completedPuzzlesUI.text = $"{completedPuzzles.Count}/{totalPuzzles}";
        }

        if (chapterProgressBar != null)
        {
            chapterProgressBar.value = totalPuzzles > 0 ? (float)completedPuzzles.Count / totalPuzzles : 0;
        }

        Debug.Log("Puzzle UI updated");
    }

    public void UpdatePuzzleDescriptionUI(string description)
    {
        puzzleDescriptionText.text = description;
    }

}
