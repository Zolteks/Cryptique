using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class UI_Quest : MonoBehaviour, ILocalizedElement
{

    [SerializeField] private GameObject puzzleDescriptionPrefab;
    [SerializeField] private Slider chapterProgressBar;

    private GameProgressionManager gameProgressionManager;
    private LanguageManager languageManager;
    void Start()
    {
        UpdatePuzzleProgress();
        UpdatePuzzleDescriptions();

        if (gameProgressionManager == null)
        {
            gameProgressionManager = GameProgressionManager.Instance;
        }
        if (languageManager == null)
        {
            languageManager = LanguageManager.Instance;
        }
        if (languageManager)
        {
            languageManager.Register(this);
        }
    }

    public void RefreshLocalized()
    {
        List<PuzzleData> puzzlesData = gameProgressionManager.GetCurrentsPuzzles();
        //Refresh the text
        foreach (Transform child in this.transform)
        {
            if (child.GetComponentInChildren<TextMeshProUGUI>())
            {
                child.GetComponentInChildren<TextMeshProUGUI>().text = puzzlesData[child.GetSiblingIndex()].GetPuzzleID();
            }
        }
    }

    private void OnDestroy()
    {
        languageManager.Unregister(this);
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
