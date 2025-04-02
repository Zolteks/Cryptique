using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private string itemID;
    private void Start()
    {
        Collect();
    }

    private void Collect()
    {
        // Notify the GameProgressionManager that an item as been collected
        GameProgressionManager.Instance.CollectItem(itemID);
        Debug.Log($"Item {itemID} collected !");

        // Notify GameManager that an item as been collected
        GameManager.GetInstance().NotifyItemCollected(itemID);

        // D�truire l'objet apr�s sa collecte
        Destroy(gameObject);
    }
}
