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
            Debug.LogWarning("pipesParent non assigné, fallback vers toute la scène");
            PipePieceTrigger[] foundPipes = Object.FindObjectsByType<PipePieceTrigger>(FindObjectsSortMode.None);
            allPipes = new List<PipePieceTrigger>(foundPipes);
        }

        RandomizePipeRotations();
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

        Instance.ShowAllJoints();

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

    private void RandomizePipeRotations()
    {
        foreach (var pipe in allPipes)
        {
            int randomAngle = Random.Range(0, 4) * 90;
            pipe.transform.rotation = Quaternion.Euler(0f, pipe.transform.eulerAngles.y, randomAngle);
        }
    }


}
