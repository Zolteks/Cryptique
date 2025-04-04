using UnityEngine;
using TMPro;
using static UnityEditor.Progress;

public class UIManager : MonoBehaviour
{
    /* Variables */
    private TextMeshProUGUI itemCountText;
    private TextMeshProUGUI itemRegionText;

    [SerializeField] private TextMeshProUGUI regionUnlockedUI;
    [SerializeField] private TextMeshProUGUI itemProgressUI;


    /* Functions */
    public void UpdateItemProgress(string itemRegion, int collectedItems, int totalItems)
    {
        if (itemProgressUI != null)
        {
            itemProgressUI.text = $"{itemRegion} Items : {collectedItems}/{totalItems}";
            Debug.Log($"Item Progress UI Updated: {itemRegion}: {collectedItems}/{totalItems}");
        }
    }

    public void UpdateRegionUnlocked(string itemRegion)
    {
        if (regionUnlockedUI != null)
        {
            regionUnlockedUI.text = $"{itemRegion}";
            Debug.Log($"Region Unlocked UI Updated: {itemRegion}");
        }
    }
}