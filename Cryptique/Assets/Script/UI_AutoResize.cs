using UnityEngine.UI;
using UnityEngine;

public class UI_AutoResize : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;

    private void Start()
    {
        // Vérifie que l'objet a un parent
        Transform parent = transform.parent.transform.parent;
        if (parent == null) return;

        //LOG parent name
        Debug.Log("Parent name: " + parent.name);

        RectTransform parentRect = parent.GetComponent<RectTransform>();
        RectTransform rectTransform = GetComponent<RectTransform>();
        if (parentRect == null || rectTransform == null) return;

        float canvasWidth = parentRect.rect.width;
        float prefabWidth = rectTransform.rect.width;

        if (canvasWidth > prefabWidth)
        {
            float percentage = (canvasWidth - prefabWidth) / canvasWidth;
            Debug.Log("Canvas is more than 1.5 times the size of the prefab");
            horizontalLayoutGroup.spacing = (canvasWidth - prefabWidth) / 2f;
        }
        else
        {
            Debug.Log("Canvas is less than 1.5 times the size of the prefab");
            horizontalLayoutGroup.spacing = 0f;
        }
    }
}
