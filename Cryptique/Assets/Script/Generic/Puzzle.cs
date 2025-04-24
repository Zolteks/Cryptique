using UnityEngine;
using UnityEngine.Audio;
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
            //Get the SFX of validation in resources
            AudioClip audioClip = Resources.Load<AudioClip>("ValidationPZL");
            var validationGroup = Resources.Load<AudioMixerGroup>("Feedback");
            SFXManager.Instance.PlaySFX(audioClip, transform.position, validationGroup);
            onSuccess?.Invoke();
            puzzleData.SetCompleted(true);
            // puzzleData.SetUnlocked(false);
      
            SaveSystemManager.Instance.GetGameData().progression.solvedPuzzles.Add(puzzleData.GetPuzzleID());

            Quit();
        }
        else
        {
            Debug.Log($"Cannot complete {puzzleData.GetPuzzleID()} because the prerequisites are not completed yet.");
        }
    }
}