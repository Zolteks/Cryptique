using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    // This function initialize a puzzle
    // If its an interfaceless UI, we'll make spawn an empty containing the appropriate script
    public static void StartPuzzle(string name)
    {
        GameObject.Instantiate(Resources.Load(name));
    }

    public void Quit()
    {
        Destroy(gameObject);
    }

    protected virtual void Complete()
    {
        Quit();
    }
}
