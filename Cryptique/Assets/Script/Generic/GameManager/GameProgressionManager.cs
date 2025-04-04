using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//  Script responsable for progression of the player
public class GameProgressionManager : MonoBehaviour
{

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

    public void CompletePuzzle(string puzzleID)
    {
        if (!completedPuzzles.Contains(puzzleID))
        {
            completedPuzzles.Add(puzzleID);
            Debug.Log($"Puzzle {puzzleID} completed !");
            CheckProgression();
        }
    }

    private void CheckProgression()
    {
        if (collectedItems.Count == 5 && completedPuzzles.Count == 5) // Exemple : if 5 objects collected and 5 Puzzle completed AdvanceChapter
        {
            AdvanceChapter();
        }
    }

    public bool IsItemCollected(string itemID)
    {
        return collectedItems.Contains(itemID);
    }

    public bool IsPuzzleCompleted(string puzzleID)
    {
        return completedPuzzles.Contains(puzzleID);
    }

    //  Next Chapter (for future)
    private void AdvanceChapter()
    {
        currentChapter++;
        Debug.Log($"Chaptre {currentChapter} unlocked !");
    }
}
