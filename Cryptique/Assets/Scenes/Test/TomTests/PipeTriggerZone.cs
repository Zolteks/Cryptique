using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PipeTriggerZone : MonoBehaviour
{
    public bool isConnected = false;

    private void Start()
    {
        // S'assure que le collider est bien un trigger
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            Debug.LogWarning($" Le collider de '{name}' doit être un trigger pour fonctionner correctement.");
            col.isTrigger = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PipeTrigger"))
        {
            Debug.Log(" Trigger touché par : " + other.name);
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
            Debug.Log(" Trigger quitté par : " + other.name);
            isConnected = false;

            PipePieceTrigger parentPipe = GetComponentInParent<PipePieceTrigger>();
            if (parentPipe != null)
            {
                parentPipe.CheckConnections();
            }
        }
    }
}
