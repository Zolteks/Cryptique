using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_IndicationEnigmes : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI puzzleIDText;

    [SerializeField]
    private PuzzleData puzzleData;

    private void Start()
    {
        if (puzzleIDText != null && puzzleData != null)
        {
            string puzzleID = puzzleData.GetPuzzleDescription();
            puzzleIDText.text = puzzleID;
        }
        else
        {
            Debug.LogWarning("Puzzle ID Text or Puzzle Data is not assigned.");
        }
    }
}