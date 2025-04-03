using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//  Script responsable for progression of the player
public class GameProgressionManager : MonoBehaviour
{
    /// <summary>
    /// /Dictonnary test for regions and number of Items per region
    /// </summary>
    private Dictionary<string, int> totalItemsPerRegion= new Dictionary<string, int>
    {
        { "Tavern", 10 },
        { "Dungeon", 5 },
        { "Forest", 3 }
    };
    /// <summary>
    /// /Dictonnary For Puzzles, probably needs another script to be more clean
    /// </summary>
    private Dictionary<string, List<string>> puzzlesByRegion = new Dictionary<string, List<string>>();

    // Dictionnaire pour stocker les descriptions des puzzles
    private Dictionary<string, string> puzzleDescriptions = new Dictionary<string, string>
{
    { "TestPuzzle", "You have to play with the man with the dices" },
    { "Puzzle2", "Solve the riddle of the old woman" },
    // Ajoutez plus de puzzles et leurs descriptions ici
};


    /* Variables */
    public static GameProgressionManager Instance;

    private HashSet<string> collectedItems = new HashSet<string>();
    private HashSet<string> itemRegion = new HashSet<string>();
    private HashSet<string> completedPuzzles = new HashSet<string>();

    private int currentChapter = 1;
    public int CollectedItemCount => collectedItems.Count;

    /* Getters and Setters */
    static public GameProgressionManager GetInstance()
    {
        return Instance;
    }

    //  Get Items and Puzzle completed for save
    public List<string> GetCollectedItems()
    {
        return new List<string>(collectedItems);
    }

    public List<string> GetRegions()
    {
        return new List<string>(itemRegion);
    }

    public List<string> GetCompletedPuzzles()
    {
        return new List<string>(completedPuzzles);
    }

    public int GetTotalItemsInRegion(string region)
    {
        if (totalItemsPerRegion.ContainsKey(region))
        {
            return totalItemsPerRegion[region];
        }
        return 0;
    }

    public string GetPuzzleDescription(string puzzleID)
    {
        if (puzzleDescriptions.ContainsKey(puzzleID))
        {
            return puzzleDescriptions[puzzleID];
        }
        return "No description available for this puzzle.";
    }


    /* Functions */

    /// <summary>
    ///  Here is the Item Logic
    /// </summary>
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameProgressionManager instances detected!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void CollectItem(string region, string itemID)
    {
        if (!collectedItems.Contains(itemID))
        {
            collectedItems.Add(itemID);
            itemRegion.Add(region);

            int totalItems = GetTotalItemsInRegion(region);
            GameManager.GetInstance().NotifyItemCollected(region, collectedItems.Count, totalItems);
        }
    }

    public bool IsItemCollected(string itemID)
    {
        return collectedItems.Contains(itemID);
    }



    /// <summary>
    ///  Here is the Puzzle Logic
    /// </summary>

    public void RegisterPuzzle(string region, string puzzleID)
    {
        if (!puzzlesByRegion.ContainsKey(region))
        {
            puzzlesByRegion[region] = new List<string>();
        }

        if (!puzzlesByRegion[region].Contains(puzzleID))
        {
            puzzlesByRegion[region].Add(puzzleID);
            Debug.Log($"Puzzle {puzzleID} added to region {region}");
        }

        // Afficher l'indice ou la description du puzzle
        string puzzleDescription = GetPuzzleDescription(puzzleID);
        Debug.Log($"Puzzle Description: {puzzleDescription}");

        // Si vous avez une UI pour afficher ce texte, vous pouvez l'afficher ici, par exemple :
        GameManager.GetInstance().NotifyPuzzleCreated(puzzleDescription);
    }

    public bool IsPuzzleCompleted(string puzzleID)
    {
        return completedPuzzles.Contains(puzzleID);
    }

    public void CompletePuzzle(string puzzleID)
    {
        if (!completedPuzzles.Contains(puzzleID))
        {
            completedPuzzles.Add(puzzleID);
            Debug.Log($"Puzzle {puzzleID} completed !");

            // Vérifiez la progression du jeu
            CheckProgression(puzzleID);
        }
    }



    /// <summary>
    ///  Here is the Chapter Logic (for future)
    /// </summary>
    private void CheckProgression(string puzzleID)
    {
        // Parcours des régions pour vérifier les énigmes complétées
        foreach (var region in puzzlesByRegion)
        {
            int completedPuzzlesCount = region.Value.Count(puzzleID => completedPuzzles.Contains(puzzleID));
            // Si tous les puzzles d'une région sont résolus, on peut vérifier si une nouvelle région doit être débloquée
            GameManager.GetInstance().NotifyPuzzleSolved(puzzleID);
        }
    }

    // Logique pour avancer le chapitre
    private void AdvanceChapter()
    {
        currentChapter++;
        Debug.Log($"Chaptre {currentChapter} unlocked !");
    }

}
