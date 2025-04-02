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
    private RectTransform m_initialGridElement;
    
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
    /// Set the selected item with the initial Grid element
    /// </summary>
    /// <param name="id">The id in the inventory system List</param>
    void ClearGridElement(int id)
    {
        RectTransform child = gContentPanel.transform.GetChild(id + 1).GetComponent<RectTransform>();
        //child = 
    }
}
