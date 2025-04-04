using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

//  Script responsable for the Entire Game logic
public class GameManager : MonoBehaviour
{
    /* Variables */
    static public GameManager Instance;

    [SerializeField] private Transform m_camera;
    [SerializeField] private UI_DialogueManager m_dialogueManager;
    [SerializeField] private UIManager uiManager;

    /* Getters and Setters */
    static public GameManager GetInstance()
    {
        return Instance;
    }

    public Transform GetCamera()
    {
        return m_camera;
    }

    public UI_DialogueManager GetDialogueManager()
    {
        return m_dialogueManager;
    }

    /* Functions */
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is multiple game manager in the scene!");
        }

        Instance = this;
    }


    public void NotifyItemCollected(string region, int collectedCount, int totalItems)
    {
        Debug.Log($"Item collected in {region}: {collectedCount}/{totalItems}");

        if (uiManager != null)
        {
            uiManager.UpdateItemProgress(region, collectedCount, totalItems);
            uiManager.UpdateRegionUnlocked(region);
        }
    }
    public void NotifyPuzzleSolved(string puzzleID)
    {
        Debug.Log($"Puzzle Solved : {puzzleID} - Update UI");

        // Notifie que le puzzle est résolu dans GameProgressionManager
        GameProgressionManager.GetInstance().CompletePuzzle(puzzleID);

        // Vérifier si d'autres puzzles sont débloqués après la résolution
        CheckAndUnlockNextPuzzles();

        if (uiManager != null)
        {
            uiManager.UpdatePuzzleProgress();  // Met à jour la progression des puzzles
        }
    }

    public void NotifyPuzzleCreated(List<string> puzzleDescriptions)
    {
        Debug.Log("Puzzle descriptions updated in UI.");

        if (uiManager != null)
        {
            uiManager.UpdatePuzzleDescriptions(puzzleDescriptions);  // Met à jour les descriptions des puzzles
        }
    }

    /// <summary>
    /// Vérifie si des puzzles sont débloqués après la résolution d'un puzzle.
    /// </summary>
    private void CheckAndUnlockNextPuzzles()
    {
        // Vous pouvez également faire un appel à GameProgressionManager pour vérifier les puzzles
        var completedPuzzles = GameProgressionManager.GetInstance().GetCompletedPuzzles();

        foreach (var puzzleID in GameProgressionManager.GetInstance().GetActivePuzzleDescriptions())
        {
            // Si le puzzle n'a pas encore été complété et qu'il est débloqué
            if (GameProgressionManager.GetInstance().CanStartPuzzle(puzzleID))
            {
                // Notifier l'UI que ce puzzle est maintenant disponible
                Debug.Log($"Puzzle {puzzleID} can now be started!");
                NotifyPuzzleCreated(GameProgressionManager.GetInstance().GetActivePuzzleDescriptions());
            }
        }
    }



    public void NotifyChapterChanged(string chapterID)
    {
        Debug.Log($"Chapter Changed : {chapterID} - Update UI or other systems");
    }

}
