using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Quest : MonoBehaviour
{

    [SerializeField] private GameObject puzzleDescriptionPrefab;
    [SerializeField] private Slider chapterProgressBar;

    void Start()
    {
        UpdatePuzzleProgress();
        UpdatePuzzleDescriptions();
    }

    public void UpdatePuzzleProgress()
    {
        var progressionManager = GameProgressionManager.Instance;
        if (progressionManager == null) return;


        int completedPuzzles = progressionManager.GetCompletedPuzzlesInRegion();
        int totalPuzzles = progressionManager.GetTotalPuzzlesInRegion();

        if (chapterProgressBar != null)
        {
            chapterProgressBar.value = totalPuzzles > 0 ? (float)completedPuzzles / totalPuzzles : 0;
        }

        Debug.Log("Puzzle UI updated");
    }

    public void UpdatePuzzleDescriptions()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        GameProgressionManager progressionManager = GameProgressionManager.Instance;

        if (progressionManager == null) return;

        List<PuzzleData> puzzles = progressionManager.GetCurrentsPuzzles();

        if (puzzles.Count == 0 ) return;


        foreach (PuzzleData puzzle in puzzles)
        {
            GameObject puzzleDescription = Instantiate(puzzleDescriptionPrefab, transform);

            TextMeshProUGUI text = puzzleDescription.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                text.text = puzzle.GetDescription();
            }
        }
    }
}
