using System.Collections.Generic;
using UnityEngine;

public class PipePieceTrigger : MonoBehaviour
{
    public List<PipeTriggerZone> connectedZones = new List<PipeTriggerZone>();
    public List<GameObject>  Joints;

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
        PipeManager.CheckVictory();
    }

    public void ShowJoints()
    {
        foreach (var joint in Joints)
        {
            joint.SetActive(true);
        }
    }
}
