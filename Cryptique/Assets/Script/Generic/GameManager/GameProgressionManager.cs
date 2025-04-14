using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameProgressionManager : Singleton<GameProgressionManager>
{
    [SerializeField] private List<ChapterData> chapters;


    /* Variables */
    private HashSet<string> collectedItems = new();
    private HashSet<string> itemRegion = new();
    private HashSet<string> completedPuzzles = new();

    private int currentChapterIndex = 0;
    private int currentRegionIndex = 0;

    public int CollectedItemCount => collectedItems.Count;

    public ChapterData GetCurrentChapter() => chapters[currentChapterIndex];

    public List<ChapterData> GetChapters() => chapters;

    public List<RegionData> GetRegions() => GetCurrentChapter().regions;

    public RegionData GetCurrentRegion() => GetRegions()[currentRegionIndex];
    public void SetCurrentRegion(int i) => currentRegionIndex = i;

    public bool IsRegionUnlocked(string regionName)
    {
        return GetAllRegions().FirstOrDefault(r => r.GetName() == regionName)?.isUnlocked ?? false;
    }

    public int GetTotalItemsInRegion(string regionName)
    {
        return GetAllRegions().FirstOrDefault(r => r.GetName() == regionName)?.totalItems ?? 0;
    }

    public int GetCollectedItemsInRegion(string regionName)
    {
        return itemRegion.Count(r => r == regionName);
    }
    private IEnumerable<RegionData> GetAllRegions()
    {
        if (chapters == null)
            Debug.LogError("Add  Scriptable Object of Chapterns in GameProgressManagers");
        foreach (var chapter in chapters)
        {
            foreach (var region in chapter.regions)
            {
                Debug.Log($"Region: {region.GetName()}");
                yield return region;
            }
        }
    }

    public bool ArePrerequisitesCompleted(PuzzleData puzzle)
    {
        foreach (var step in puzzle.GetPrerequisites())
        {
            if(step == null)
                continue;

            if(step.IsCompleted())
                continue;
            else
                return false;
        }
        return true;
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

    //private void CheckProgression(string solvedPuzzleID)
    //{
    //    foreach (var step in puzzleSteps)
    //    {
    //        if (IsPuzzleCompleted(step.puzzleID))
    //            continue;

    //        if (IsPuzzleAvailable(step.puzzleID))
    //        {
    //            RegisterPuzzle("Tavern", step.puzzleID);
    //            Debug.Log($"Next puzzle unlocked: {step.puzzleID}");
    //        }
    //    }
    //}

    //private bool IsPuzzleAvailable(string puzzleID)
    //{
    //    var step = puzzleSteps.Find(s => s.puzzleID == puzzleID);
    //    if (step == null)
    //        return true;

    //    return step.requiredPuzzles.All(p => completedPuzzles.Contains(p));
    //}
}
