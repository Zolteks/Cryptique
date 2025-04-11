using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    [SerializeField] private string puzzleID;
    [SerializeField] private string region;
    [SerializeField] private UnityEvent onSuccess;

    private void Start()
    {
        Debug.Log($"Registering puzzle {puzzleID} in region {region}");
        GameProgressionManager.Instance.RegisterPuzzle(region, puzzleID);
    }

    public static void StartPuzzle(string name)
    {
        GameObject.Instantiate(Resources.Load("Puzzles/PZL_"+name));
    }

    public static void StartPuzzle(string name, string region, UnityEvent onSuccess)
    {
        GameObject pzlGo = (GameObject)GameObject.Instantiate(Resources.Load("Puzzles/PZL_" + name));
        Puzzle pzl = pzlGo.GetComponent<Puzzle>();
        pzl.puzzleID = name;
        pzl.region = region;
        pzl.onSuccess = onSuccess;
    }

    public void Quit()
    {
        Destroy(gameObject);
    }


    protected virtual void Complete()
    {
        if (GameProgressionManager.Instance.ArePrerequisitesCompleted(puzzleID))
        {
            Debug.Log($"{puzzleID} is completed.");
            GameProgressionManager.Instance.CompletePuzzle(puzzleID);
            onSuccess?.Invoke();

            //SaveSystemManager.Instance.GetGameData().collectedItems.Add(puzzleID);

            Quit();
        }
        else
        {
            Debug.Log($"Cannot complete {puzzleID} because the prerequisites are not completed yet.");
        }
    }
}