using System.Collections.Generic;
using UnityEngine;

public class PipeManager : MonoBehaviour
{
    public List<PipePieceTrigger> allPipes = new List<PipePieceTrigger>();
    public static PipeManager Instance;

    private void Awake()
    {
        Instance = this;
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
        }
        else
        {
            Debug.Log(" Not won yet ");
        }
    }
}
