using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IN_PuzzleLauncher : OBJ_Interactable
{
    [SerializeField] private PuzzleData puzzleData;
    [SerializeField] private UnityEvent onSuccess;

    //private void OnMouseDown()
    //{
    //    if (false == CanInteract()) return;

    //    Interact();
    //}
    private PC_PlayerController m_playerController;

    private void Awake()
    {
        m_playerController = PC_PlayerController.Instance;
    }

    public override bool Interact()
    {
        m_playerController.MoveForInteraction();

        return true;
    }

    private void OnEnable()
    {
        m_playerController.OnInteractionCallback += Wait;
    }

    private void OnDisable()
    {
        m_playerController.OnInteractionCallback -= Wait;
    }

    void Wait()
    {
        Debug.Log("puzzle started");
        Puzzle.StartPuzzle(puzzleData, onSuccess);
    }
}
