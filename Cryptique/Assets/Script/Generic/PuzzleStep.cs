using System;
using System.Collections.Generic;

[System.Serializable]
public class PuzzleStep
{
    public string puzzleID;
    public List<string> requiredPuzzles = new List<string>();
}
