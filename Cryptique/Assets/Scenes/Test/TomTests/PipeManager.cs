using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    public List<PipePieceTrigger> allPipes = new List<PipePieceTrigger>();
    public Transform pipesParent;
    public static PipeManager Instance;
    public static PZL_GutterLabyrinth PZL_GutterLabyrinth;
    public bool isSolved = false;

    private void Awake()
    {
        Instance = this;

        if (pipesParent != null)
        {
            PipePieceTrigger[] foundPipes = pipesParent.GetComponentsInChildren<PipePieceTrigger>(true);
            allPipes = new List<PipePieceTrigger>(foundPipes);
        }
        else
        {
            Debug.LogWarning("pipesParent n'est pas assigné dans PipeManager.");
        }
    }

    public static void CheckVictory()
    {
        int totalValidConnections = 0;
        int totalPipeCount = Instance.allPipes.Count;

        foreach (var pipe in Instance.allPipes)
        {
            foreach (var zone in pipe.connectedZones)
            {
                if (zone.isConnected)
                {
                    totalValidConnections++;
                }
            }
        }

        int actualConnections = totalValidConnections / 2;

        Debug.Log($"Actual connexions  : {actualConnections} / Required : {totalPipeCount - 1}");

        if (actualConnections == totalPipeCount - 1)
        {
            Debug.Log(" Victory !");
            Win();
        }
        else
        {
            Debug.Log(" Not won yet ");
        }
    }

    private static void Win()
    {
        Instance.isSolved = true;

        // Affiche tous les joints des pipes
        Instance.ShowAllJoints();

        // Résout le puzzle
        if (PZL_GutterLabyrinth == null)
            PZL_GutterLabyrinth = Object.FindAnyObjectByType<PZL_GutterLabyrinth>();

        if (PZL_GutterLabyrinth != null)
            PZL_GutterLabyrinth.Solve();
    }



    public void ShowAllJoints()
    {
        foreach (var pipe in allPipes)
        {
            pipe.ShowJoints();
        }
    }

}
