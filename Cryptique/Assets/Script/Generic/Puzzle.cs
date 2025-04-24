using UnityEngine;
using UnityEngine.Events;

public class Puzzle : MonoBehaviour
{
    [SerializeField] public PuzzleData puzzleData;
    [SerializeField] private UnityEvent onSuccess;

    public static void StartPuzzle(PuzzleData data, UnityEvent onSuccess)
    {
        GameObject pzlGo = (GameObject)GameObject.Instantiate(Resources.Load("Puzzles/PZL_" + data.defaultPuzzleID));
        Puzzle pzl = pzlGo.GetComponent<Puzzle>();
        pzl.onSuccess = onSuccess;
        pzl.puzzleData = data;
    }

    public virtual void Quit()
    {
        PC_PlayerController.Instance.EnableInput();
        SGL_InteractManager.Instance.ChangeCamera(Camera.main);
        Destroy(gameObject);
    }


    protected virtual void Complete()
    {
        if (GameProgressionManager.Instance.ArePrerequisitesCompleted(puzzleData))
        {
            Debug.Log($"{puzzleData.GetPuzzleID()} is completed.");
            onSuccess?.Invoke();
            puzzleData.SetCompleted(true);
            // puzzleData.SetUnlocked(false);
            print(SaveSystemManager.Instance.gameObject.name);
            print(SaveSystemManager.Instance.GetGameData());
            print(SaveSystemManager.Instance.GetGameData().progression);
            print(SaveSystemManager.Instance.GetGameData().progression.solvedPuzzles);
            print(puzzleData);
            print(puzzleData.GetPuzzleID());
            SaveSystemManager.Instance.GetGameData().progression.solvedPuzzles.Add(puzzleData.GetPuzzleID());

            Quit();
        }
        else
        {
            Debug.Log($"Cannot complete {puzzleData.GetPuzzleID()} because the prerequisites are not completed yet.");
        }
    }
}