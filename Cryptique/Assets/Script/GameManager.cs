using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static private GameManager m_instance;

    [SerializeField] Transform m_camera;
    [SerializeField] UI_DialogueManager m_dialogueManager;

    private void Awake()
    {
        if(m_instance != null)
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
}
