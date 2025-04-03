using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ManageInventory : MonoBehaviour
{
    // Singleton
    public static UI_ManageInventory Instance;
    
    /* Variables */
    public GameObject gContentPanel;
    public Scrollbar scScrollbarPanel;
    
    private List<Vector3> m_PositionGrid = new();
    private List<string> m_NameGrid = new();
    
    [SerializeField]
    private Sprite m_initialSprite;
    
    /* Functions */
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Start()
    {
        // Forcer la mise � jour imm�diate de tous les Canvas et leurs LayoutGroups
        Canvas.ForceUpdateCanvases();

        // R�cup�rer tous les RectTransform des enfants
        RectTransform[] allChildren = gContentPanel.GetComponentsInChildren<RectTransform>();

        scScrollbarPanel.value = 1;

        for (int i = 1; i < allChildren.Length; i += 2)
        {
            RectTransform child = allChildren[i];

            m_PositionGrid.Add(child.anchoredPosition);
            m_NameGrid.Add(child.name);

            Debug.Log(child.name + " : " + child.anchoredPosition);
        }

        print(m_PositionGrid);
        print(m_NameGrid);
    }
    
    /// <summary>
    /// Update the content panel with the items in the inventory
    /// </summary>
    /// <param name="items">The list of items in the inventory</param>
    public void UpdateContentPanel(List<OBJ_Item> items)
    {
        int allChildren = gContentPanel.GetComponentsInChildren<RectTransform>().Length;
        int allItems = items.Count;
        for (int i = 0; i < allChildren; i++)
        {
            if (i < allItems)
                UpdateGridElement(i, items[i]);
            else
                ClearGridElement(i);
        }
    }
    
    /// <summary>
    /// Set the selected item with the initial Grid element
    /// </summary>
    /// <param name="id">The id of the child to be changed</param>
    /// <param name="item">The item added to the inventory</param>
    public void UpdateGridElement(int id, OBJ_Item item)
    {
        Image child = gContentPanel.transform.GetChild(id).GetChild(0).GetComponent<Image>();
        child.sprite = item.GetSprite();
        child.color = new Color(255, 255 ,255);
    }
    
    /// <summary>
    /// Set the selected item with the initial Grid element
    /// </summary>
    /// <param name="id">The id in the inventory system List</param>
    public void ClearGridElement(int id)
    {
        Image child = gContentPanel.transform.GetChild(id).GetChild(0).GetComponent<Image>();
        child.sprite = m_initialSprite;
        child.color = new Color(255, 0 ,0);
    }
}
