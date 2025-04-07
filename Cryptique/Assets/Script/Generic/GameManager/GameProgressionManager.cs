using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameProgressionManager : MonoBehaviour
{

    /// <summary>
    /// /Dictonnary For Puzzles, probably needs another script to be more clean
    /// </summary>
    private Dictionary<string, List<string>> puzzlesByRegion = new Dictionary<string, List<string>>();
    private Dictionary<string, string> puzzleDescriptions = new Dictionary<string, string>
    {
        { "PuzzleSpaceBar", "You have to Press SpaceBar" },
        { "PuzzleB", "You have to Press B" },
        { "PuzzleC", "You have to Press C" },
        { "PuzzleD", "You have to Press D" },
        { "PuzzleE", "You have to Press E" }
    };

    [SerializeField] private List<string> chapters = new List<string>
    {
        "Wendigo",
        "Kelpie",
        "Chupacabra",
    };

    /// <summary>
    /// Dictionary test for regions and items
    /// </summary>
    private Dictionary<string, List<string>> regions = new Dictionary<string, List<string>>
    {
        { "Wendigo" , new List<string> { "Tavern", "Village", "Forest", "Cave" } },
        { "Kelpie" , new List<string> { "Tavern", "Village", "Forest", "Cave" } },
        { "Chupacabra" , new List<string> { "Tavern", "Village", "Forest", "Cave" } },
    };

    /// <summary>
    /// /Dictonnary test for regions and number of Items per region for all chapters
    /// </summary>
    private Dictionary<string, int> totalItemsPerRegion= new Dictionary<string, int>
    {
        { "Tavern", 10 },
        { "Village", 5 },
        { "Forest", 3 },
        { "Cave", 7 },
    };

    /// <summary>
    /// Dictionary test for set unlocked regions
    /// </summary>
    private Dictionary<string, bool> regionUnlocked = new Dictionary<string, bool>
    {
        { "Tavern", true },
        { "Village", false },
        { "Forest", false },
        { "Cave", false },
    };

    /* Variables */
    public static GameProgressionManager Instance;
    [SerializeField]
    private List<PuzzleStep> puzzleSteps = new List<PuzzleStep>();

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

    public List<string> GetCollectedItems()
    {
        return new List<string>(collectedItems);
    }

    public int GetTotalItemsInRegion(string region)
    {
        if (totalItemsPerRegion.ContainsKey(region))
        {
            return totalItemsPerRegion[region];
        }
        return 0;
    }
    
    // Get Chapter
    public string GetCurrentChapter()
    {
        return chapters[currentChapter - 1];
    }

    public List<string> GetChapters()
    {
        return new List<string>(chapters);
    }

    // Get region
    public List<string> GetRegions(string chapter)
    {
        if (regions.ContainsKey(chapter))
        {
            return regions[chapter];
        }
        return new List<string>();
    }

    public bool IsRegionUnlocked(string region)
    {
        if (regionUnlocked.ContainsKey(region))
        {
            return regionUnlocked[region];
        }
        return false;
    }

    public List<string> GetRegions()
    {
        return new List<string>(itemRegion);
    }

    public List<string> GetCompletedPuzzles()
    {
        return new List<string>(completedPuzzles);
    }

    public string GetPuzzleDescription(string puzzleID)
    {
        if (puzzleDescriptions.ContainsKey(puzzleID))
        {
            return puzzleDescriptions[puzzleID];
        }
        return "No description available for this puzzle.";
    }

    public List<string> GetActivePuzzleDescriptions()
    {
        List<string> activeDescriptions = new List<string>();

        foreach (var region in puzzlesByRegion)
        {
            foreach (var puzzleID in region.Value)
            {
                if (!completedPuzzles.Contains(puzzleID))
                {
                    activeDescriptions.Add(GetPuzzleDescription(puzzleID));
                }
            }
        }

        return activeDescriptions;
    }

    /* Functions */
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameProgressionManager instances detected!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
      //  DontDestroyOnLoad(gameObject);
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

        string puzzleDescription = GetPuzzleDescription(puzzleID);
        Debug.Log($"Puzzle Description: {puzzleDescription}");

        List<string> activeDescriptions = GetActivePuzzleDescriptions();
        GameManager.GetInstance().NotifyPuzzleCreated(activeDescriptions);
    }

    public bool IsPuzzleCompleted(string puzzleID)
    {
        return completedPuzzles.Contains(puzzleID);
    }

    public void CompletePuzzle(string puzzleID)
    {
        if (!completedPuzzles.Contains(puzzleID))
        {
            bool canCompletePuzzle = true;
            foreach (var step in puzzleSteps)
            {
                if (step.nextPuzzleID == puzzleID)
                {
                    if (!step.requiredPuzzles.All(p => completedPuzzles.Contains(p)))
                    {
                        canCompletePuzzle = false;
                        Debug.Log($"Cannot complete puzzle {puzzleID} because required puzzles are not completed.");
                        break;
                    }
                }
            }

            if (canCompletePuzzle)
            {
                completedPuzzles.Add(puzzleID);
                Debug.Log($"Puzzle {puzzleID} completed!");

                GameManager.GetInstance().NotifyPuzzleSolved(puzzleID);
                CheckProgression(puzzleID);
            }
        }
    }

    public bool ArePrerequisitesCompleted(string puzzleID)
    {
        foreach (var step in puzzleSteps)
        {
            if (step.nextPuzzleID == puzzleID)
            {
                if (!step.requiredPuzzles.All(p => completedPuzzles.Contains(p)))
                {
                    return false;
                }
            }
        }
        return true;
    }

    private void CheckProgression(string solvedPuzzleID)
    {
        foreach (var step in puzzleSteps)
        {
            if (IsPuzzleCompleted(step.nextPuzzleID))
                continue;

            if (step.requiredPuzzles.All(p => completedPuzzles.Contains(p)))
            {
                RegisterPuzzle("Tavern", step.nextPuzzleID);
                Debug.Log($"Next puzzle unlocked: {step.nextPuzzleID}");
            }
        }
    }
    public bool CanStartPuzzle(string puzzleID)
    {
        foreach (var step in puzzleSteps)
        {
            if (step.nextPuzzleID == puzzleID)
            {
                return step.requiredPuzzles.All(p => completedPuzzles.Contains(p));
            }
        }

        return true;
    }

    private void CheckProgression()
    {
        if (collectedItems.Count == 5 && completedPuzzles.Count == 5) // Exemple : if 5 objects collected and 5 Puzzle completed AdvanceChapter
        {
            AdvanceChapter();
        }
    }

    //  Next Chapter (for future)
    private void AdvanceChapter()
    {
        currentChapter++;
        Debug.Log($"Chaptre {currentChapter} unlocked !");
    }
}
