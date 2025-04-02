using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class GameProgressionManager : MonoBehaviour
{
    public static GameProgressionManager Instance;

    private HashSet<string> collectedItems = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameProgressionManager instances detected!");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    static public GameProgressionManager GetInstance()
    {
        return Instance;
    }
    public void CollectItem(string itemID)
    {
        if (!collectedItems.Contains(itemID))
        {
            collectedItems.Add(itemID);
            Debug.Log($"Item {itemID} add to items collected !");
        }
    }

    // Vérification si un objet est collecté
    public bool IsItemCollected(string itemID)
    {
        return collectedItems.Contains(itemID);
    }
}
