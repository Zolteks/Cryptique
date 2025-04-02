using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemCountText;

    public void UpdateItemCount(int itemCount)
    {
        if (itemCountText != null)
        {
            itemCountText.text = "Items collectés : " + itemCount.ToString();
        }
    }
}