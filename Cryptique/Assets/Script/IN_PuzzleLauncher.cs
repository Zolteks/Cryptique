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
        m_playerController.OnInteractionCallback += Wait;

        return true;
    }

    void Wait()
    {
        Puzzle.StartPuzzle(puzzleData, onSuccess);
        m_playerController.OnInteractionCallback -= Wait;
    }
}
