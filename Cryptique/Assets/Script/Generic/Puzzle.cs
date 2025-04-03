using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField] private string puzzleID;
    [SerializeField] private string region;

    private void Start()
    {
        GameProgressionManager.Instance.RegisterPuzzle(region, puzzleID);
    }

    public static void StartPuzzle(string name)
    {
        GameObject.Instantiate(Resources.Load(name));
    }

    public void Quit()
    {
        Destroy(gameObject);
    }

    protected virtual void Complete()
    {
        GameProgressionManager.Instance.CompletePuzzle(puzzleID);
        Quit();
    }
}
