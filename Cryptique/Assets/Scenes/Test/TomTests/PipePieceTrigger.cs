using System.Collections.Generic;
using UnityEngine;

public class PipePieceTrigger : MonoBehaviour
{
    public List<PipeTriggerZone> connectedZones = new List<PipeTriggerZone>();

    public void CheckConnections()
    {
        int validConnections = 0;

        foreach (var zone in connectedZones)
        {
            if (zone.isConnected)
            {
                validConnections++;
            }
        }

        Debug.Log("Number of valid connexions : " + validConnections);
        PipeManager.CheckVictory();
    }
}
