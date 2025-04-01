using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{

    public GameObject gInventoryPanel;
    public GameObject gInventoryColumnPanel;
    public Button bInventoryButton;

    // Start is called before the first frame update
    void Start()
    {
        gInventoryPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInventory()
    {
        if (gInventoryPanel.activeSelf)
        {
            gInventoryPanel.SetActive(false);
        }
        else
        {
            gInventoryPanel.SetActive(true);
        }
    }
}
