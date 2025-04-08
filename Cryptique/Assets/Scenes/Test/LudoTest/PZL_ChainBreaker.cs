using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PZL_ChainBreaker : MonoBehaviour
{
    [SerializeField] private GameObject goChain;

    private OBJ_Item objiSelectedItem;

    private void OnEnable()
    {
        if (DragAndDrop.Instance != null)
            DragAndDrop.Instance.OnDragEnded += OnDragEnded;
            DragAndDrop.Instance.OnInteractObject += OnInteractObject;
    }

    private void OnDisable()
    {
        if (DragAndDrop.Instance != null)
            DragAndDrop.Instance.OnDragEnded -= OnDragEnded;
            DragAndDrop.Instance.OnInteractObject -= OnInteractObject;
    }

    private void OnDragEnded(OBJ_Item selectedItem, GameObject draggedObject)
    {
        if (selectedItem != null && draggedObject != null)
        {
            objiSelectedItem = selectedItem;
        }
    }

    private void OnInteractObject(GameObject objectToInteract)
    {
        if (objectToInteract != null && objiSelectedItem != null)
        {
            if (objiSelectedItem.name == "Pince" && objectToInteract.name == "Chaine")
            {
                goChain.SetActive(false);
                print("GG");
            }
            
        }
    }
}
