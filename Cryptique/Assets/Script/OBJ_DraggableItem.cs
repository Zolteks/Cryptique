using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OBJ_DraggableItem : MonoBehaviour
{
    /* Variables */
    [SerializeField]
    private OBJ_Item m_item;
    
    /* Getters and Setters */
    public void SetItem(OBJ_Item item) => m_item = item;
    public OBJ_Item GetItem() => m_item;
}
