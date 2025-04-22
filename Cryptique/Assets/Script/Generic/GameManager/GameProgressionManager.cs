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

    private int currentChapterIndex = 0;
    private int currentRegionIndex = 0;

    public int CollectedItemCount => collectedItems.Count;

    private void Start()
    {
        foreach (var item in GetCurrentRegion().GetPuzzles())
        {
            item.SetCompleted(false);
        }
    }

    public ChapterData GetCurrentChapter() => chapters[currentChapterIndex];

    public List<ChapterData> GetChapters() => chapters;

    public List<RegionData> GetRegions() => GetCurrentChapter().regions;

    public RegionData GetCurrentRegion() => GetRegions()[currentRegionIndex];
    public void SetCurrentRegion(int i) => currentRegionIndex = i;

    public List<PuzzleData> GetCurrentsPuzzles()
    {
        var region = GetCurrentRegion();
        if (region == null)
            return null;
        var puzzles = region.GetPuzzles();
        if (puzzles == null || puzzles.Count == 0)
            return null;

        // Find all puzzle that is not completed and has prerequisites completed
        var availablePuzzles = new List<PuzzleData>();

        foreach (var puzzle in puzzles)
        {
            if ((!puzzle.IsCompleted() && ArePrerequisitesCompleted(puzzle)) || puzzle.IsUnlocked())
            {
                availablePuzzles.Add(puzzle);
                List<HintData> hints = puzzle.GetHints();
                for (int i = 0; i < hints.Count; i++)
                {
                    if (hints[i].IsUnlocked())
                        continue;
                    hints[i].SetTimeToShow(hints[i].GetTimeToShow() * i);
                    hints[i].StartTimeToShow(this);
                }
            }
        }

        return availablePuzzles;
    }

    public RegionData GetRegionByName(string name)
    {
        foreach (RegionData reg in GetRegions())
        {
            if (reg.GetName() == name)
                return reg;
        }
        return null;
    }

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

    public int GetTotalPuzzlesInRegion()
    {
        return GetAllRegions().FirstOrDefault(r => r.GetName() == GetCurrentRegion().GetName())?.GetPuzzles().Count ?? 0;
    }

    public int GetCompletedPuzzlesInRegion()
    {
        return GetAllRegions().FirstOrDefault(r => r.GetName() == GetCurrentRegion().GetName())?.GetCompletedPuzzlesCount() ?? 0;
    }

    public bool ArePrerequisitesCompleted(PuzzleData puzzle)
    {
        List<PuzzleData> prerequisites = puzzle.GetPrerequisites();
        if (prerequisites == null || prerequisites.Count == 0)
            return true;
       
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
}
