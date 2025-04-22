using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Pseudo;

public class IN_DropMirrorLauncher : OBJ_InteractOnDrop
{
    [SerializeField] GameObject Border;

    [SerializeField] private PuzzleData puzzleData;
    [SerializeField] private UnityEvent onSuccess;
    

    public override bool Interact()
    {
        Puzzle.StartPuzzle(puzzleData, onSuccess);

        GameObject mirror = GameObject.Find("PZL_Lake(Clone)");
        if (mirror == null)
        {
            Debug.LogError("Mirror not found");
            return false;
        }

        GameObject parentObject = GameObject.Find("PZL_UndergroundLakePuzzle");
        if (parentObject == null)
        {
            Debug.LogError("Parent object 'PZL_UndergroundLakePuzzle' not found");
            return false;
        }

        mirror.transform.SetParent(parentObject.transform);

        mirror.transform.localPosition = Border.transform.localPosition;

        Border.SetActive(false);

        return true;
    }

}