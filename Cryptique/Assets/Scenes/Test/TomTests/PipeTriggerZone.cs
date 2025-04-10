using UnityEngine;

public class PipeTriggerZone : MonoBehaviour
{
    public bool isConnected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PipeTrigger"))
        {
            Debug.Log("Trigger touched by : " + other.name);
            isConnected = true;

            PipePieceTrigger parentPipe = GetComponentInParent<PipePieceTrigger>();
            if (parentPipe != null)
            {
                parentPipe.CheckConnections();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PipeTrigger"))
        {
            isConnected = false;

            PipePieceTrigger parentPipe = GetComponentInParent<PipePieceTrigger>();
            if (parentPipe != null)
            {
                parentPipe.CheckConnections();
            }
        }
    }
}
