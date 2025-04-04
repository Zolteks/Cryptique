using System.Collections.Generic;

[System.Serializable]
public class PuzzleStep
{
    public List<string> requiredPuzzles;
    public string nextPuzzleID;
}