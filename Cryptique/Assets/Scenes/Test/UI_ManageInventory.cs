using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_ManageInventory : MonoBehaviour
{
    public GameObject gContentPanel;
    
    private List<Vector3> lPositionGrid = new List<Vector3>();
    private List<string> lNameGrid = new List<string>();

    void Start()
    {
        // Forcer la mise à jour immédiate de tous les Canvas et leurs LayoutGroups
        Canvas.ForceUpdateCanvases();

        // Récupérer tous les RectTransform des enfants
        RectTransform[] allChildren = gContentPanel.GetComponentsInChildren<RectTransform>();

        for (int i = 1; i < allChildren.Length; i += 2)
        {
            RectTransform child = allChildren[i];

            lPositionGrid.Add(child.anchoredPosition);
            lNameGrid.Add(child.name);

        }

        print(lPositionGrid);
        print(lNameGrid);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < lPositionGrid.Count; i++)
        {
            print(lNameGrid[i] + " : " + lPositionGrid[i]);
        }
    }
}
