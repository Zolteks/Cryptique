using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    [SerializeField] private PuzzleData puzzleData;
    [SerializeField] private UnityEvent onSuccess;

    private void Start()
    {
        //Debug.Log($"Registering puzzle {puzzleData.GetPuzzleID(} in region {region}");
        //GameProgressionManager.Instance.RegisterPuzzle(region, puzzleID);
    }
    public static void StartPuzzle(PuzzleData data, UnityEvent onSuccess)
    {
        GameObject pzlGo = (GameObject)GameObject.Instantiate(Resources.Load("Puzzles/PZL_" + data.defaultPuzzleID));
        Puzzle pzl = pzlGo.GetComponent<Puzzle>();
        pzl.onSuccess = onSuccess;
        pzl.puzzleData = data;
    }

    public void Quit()
    {
        Destroy(gameObject);
    }


    protected virtual void Complete()
    {
        if (GameProgressionManager.Instance.ArePrerequisitesCompleted(puzzleData))
        {
            Debug.Log($"{puzzleData.GetPuzzleID()} is completed.");
            onSuccess?.Invoke();
            puzzleData.SetCompleted(true);

            //SaveSystemManager.Instance.GetGameData().collectedItems.Add(puzzleID);
            Quit();
        }
        else
        {
            Debug.Log($"Cannot complete {puzzleData.GetPuzzleID()} because the prerequisites are not completed yet.");
        }
    }
}