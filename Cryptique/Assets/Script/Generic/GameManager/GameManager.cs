using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

//  Script responsable for the Entire Game logic
public class GameManager : MonoBehaviour
{
    /* Variables */
    static public GameManager Instance;

    [SerializeField] private Transform m_camera;
    [SerializeField] private UI_DialogueManager m_dialogueManager;
    [SerializeField] private UIManager uiManager;

    /* Getters and Setters */
    static public GameManager GetInstance()
    {
        return Instance;
    }

    public Transform GetCamera()
    {
        return m_camera;
    }

    public UI_DialogueManager GetDialogueManager()
    {
        return m_dialogueManager;
    }

    /* Functions */
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is multiple game manager in the scene!");
        }

        Instance = this;
    }

    public void NotifyChapterChanged(string chapterID)
    {
        Debug.Log($"Chapter Changed : {chapterID} - Update UI or other systems");
    }


    public void NotifyItemCollected(string itemID, int itemCount)
    {
        Debug.Log($"Item collected: {itemID} - Total: {itemCount} - Update UI or other systems");

        if (uiManager != null)
        {
            uiManager.UpdateItemCount(itemCount); // Update UI like : "Tavern 0/2"
        }
    }


    public void NotifyPuzzleSolved(string PuzzleID)
    {
        //  work to do link with the progress bar
        Debug.Log($"Puzzle Solved : {PuzzleID} - Update UI or other systems");
    }
}
