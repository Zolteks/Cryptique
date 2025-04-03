using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//  Script responsable for progression of the player
public class GameProgressionManager : MonoBehaviour
{

    private Dictionary<string, int> totalItemsPerRegion= new Dictionary<string, int>
{
    { "Tavern", 10 },
    { "Dungeon", 5 },
    { "Forest", 3 }
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

    //  Next Chapter
    private void AdvanceChapter()
    {
        currentChapter++;
        Debug.Log($"Chaptre {currentChapter} unlocked !");
    }
}
