using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ManageInventory : MonoBehaviour
{
    public GameObject gContentPanel;
    public Scrollbar scScrollbarPanel;
    
    private List<Vector3> lPositionGrid = new List<Vector3>();
    private List<string> lNameGrid = new List<string>();

    void Start()
    {
        // Forcer la mise à jour immédiate de tous les Canvas et leurs LayoutGroups
        Canvas.ForceUpdateCanvases();

        // Récupérer tous les RectTransform des enfants
        RectTransform[] allChildren = gContentPanel.GetComponentsInChildren<RectTransform>();

        scScrollbarPanel.value = 1;

        for (int i = 1; i < allChildren.Length; i += 2)
        {
            RectTransform child = allChildren[i];

            lPositionGrid.Add(child.anchoredPosition);
            lNameGrid.Add(child.name);

            Debug.Log(child.name + " : " + child.anchoredPosition);
        }

        print(lPositionGrid);
        print(lNameGrid);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lPositionGrid.Count; i++)
        {
            //print(lNameGrid[i] + " : " + lPositionGrid[i]);
        }
    }
}
