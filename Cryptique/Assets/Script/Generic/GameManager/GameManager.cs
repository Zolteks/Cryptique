using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager m_instance;

    [SerializeField] private Transform m_camera;
    [SerializeField] private UI_DialogueManager m_dialogueManager;

    private void Awake()
    {
        if (m_instance != null)
        {
            Debug.LogError("There is multiple game manager in the scene!");
        }

        m_instance = this;
    }

    static public GameManager GetInstance()
    {
        return m_instance;
    }

    public Transform GetCamera()
    {
        return m_camera;
    }

    public UI_DialogueManager GetDialogueManager()
    {
        return m_dialogueManager;
    }

    // Cette m�thode est utilis�e pour notifier que l'objet a �t� collect�
    public void NotifyItemCollected(string itemID)
    {
        // Vous pouvez ici mettre � jour l'UI, ou faire autre chose en fonction de l'objet collect�
        Debug.Log($"Item collect�: {itemID} - Mettre � jour l'UI ou d'autres syst�mes");
    }
}
