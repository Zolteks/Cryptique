using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;

public class UI_Quest : MonoBehaviour, ILocalizedElement
{

    [SerializeField] private GameObject puzzleDescriptionPrefab;
    [SerializeField] private Slider chapterProgressBar;

    private GameProgressionManager gameProgressionManager;
    private LanguageManager languageManager;

    int lastUpdatedAmount = 0;

    void Start()
    {

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

    public void RefreshLocalized(LanguageCode cLanguage)
    {
        List<PuzzleData> puzzlesData = gameProgressionManager.GetCurrentsPuzzles();
        if (puzzlesData == null) return;
        //Refresh the text
        foreach (Transform child in this.transform)
        {
            if (child.GetComponentInChildren<TextMeshProUGUI>())
            {
                Debug.Log("Quest :" + child.GetComponentInChildren<TextMeshProUGUI>().text);
                child.GetComponentInChildren<TextMeshProUGUI>().text = puzzlesData[child.GetSiblingIndex()].GetDescription(cLanguage);
                Debug.Log("Quest :" + child.GetComponentInChildren<TextMeshProUGUI>().text);
            }
        }
    }

    private void OnDestroy()
    {
        languageManager.Unregister(this);
    }

    public void UpdateAll()
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

        StartCoroutine(CoroutineUpdateProgression(lastUpdatedAmount, completedPuzzles, totalPuzzles));

        lastUpdatedAmount = completedPuzzles;

        Debug.Log("Puzzle UI updated");
    }

    IEnumerator CoroutineUpdateProgression(int from, int to, int totalPzl)
    {
        float amount = (float)from / totalPzl;
        float targetAmount = (float)to / totalPzl;
        float baseAmount = amount;
        chapterProgressBar.value = Mathf.Max(0f, from);
        while(amount  < targetAmount)
        {
            amount += Mathf.Min(Mathf.Lerp(0, targetAmount - baseAmount, .5f * Time.deltaTime), targetAmount - amount);
            chapterProgressBar.value = amount;
            yield return null;
        }
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


        LanguageCode currentLanguage = SaveSystemManager.Instance.GetGameData().settings.langue;

        foreach (PuzzleData puzzle in puzzles)
        {
            string description = puzzle.GetDescription(currentLanguage);
            if (string.IsNullOrWhiteSpace(description))
                continue;

            GameObject puzzleDescription = Instantiate(puzzleDescriptionPrefab, transform);
            TextMeshProUGUI text = puzzleDescription.GetComponentInChildren<TextMeshProUGUI>();
            if (text != null)
            {
                Debug.Log("Puzzle ID: " + puzzle.GetPuzzleID());
                text.text = description;
            }
        }
    }
}
