using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test_PlayerInteraction : OBJ_Interactable
{
    [SerializeField] private PC_PlayerController m_playerController;
    
    private void Awake()
    {
        foreach (var rootGameObject in SceneManager.GetActiveScene().GetRootGameObjects())
        {
            var script = rootGameObject.GetComponent<PC_PlayerController>();
            if (script != null)
                m_playerController = script;
        }
        if (m_playerController == null)
            Debug.LogError("PlayerController not found");
    }
    
    public override bool Interact()
    {
        Debug.Log("Interact");
        m_playerController.MoveAndInteract(gameObject.transform.position);
        return true;
    }
}
