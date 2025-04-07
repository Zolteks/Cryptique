using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class OBJ_Item : ScriptableObject
{
    /* Variables */
    [SerializeField]
    private string m_itemName;
    [SerializeField]
    private string m_itemRegion;
    [SerializeField]
    private GameObject m_itemPrefab;
    [SerializeField]
    private Sprite m_itemSprite;
    
    private string m_itemDescription;

    /* Getters and Setters */
    public string GetName() => m_itemName;
    public string GetRegion() => m_itemRegion;
    public string GetDescription() => m_itemDescription;
    public GameObject GetPrefab() => m_itemPrefab;
    public Sprite GetSprite() => m_itemSprite;
    public void SetName(string name) => m_itemName = name;
    public void SetRegion(string region) => m_itemRegion = region;
    public void SetDescription(string description) => m_itemDescription = description;
    public void SetPrefab(GameObject prefab) => m_itemPrefab = prefab;
    
    public bool UseItemOn(GameObject ObjectToInteract)
    {
        if (!ObjectToInteract.TryGetComponent<OBJ_Collectable>(out var objInteractable))
            return false;
        
        objInteractable.UseOn(ObjectToInteract);
        return true;
    }
}
