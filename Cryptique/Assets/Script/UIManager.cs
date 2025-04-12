using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Must be attached to a GameObject with a Canvas component and a GraphicRaycaster.
/// </summary>
public class UIManager : Singleton<UIManager>
{
    /* Variables */
    [Header("Basics UI")]
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private GraphicRaycaster m_graphicRaycaster;
    [SerializeField] private GameObject m_inventoryUI;
    
    /* Getters */
    public Canvas GetCanvas() => m_canvas;
    public GraphicRaycaster GetGraphicRaycaster() => m_graphicRaycaster;
    public GameObject GetInventoryUI() => m_inventoryUI;
    
    /* Functions */
    private void Awake()
    {
        m_canvas = GetComponent<Canvas>();
        m_graphicRaycaster = GetComponent<GraphicRaycaster>();
        m_inventoryUI = transform.GetChild(0).gameObject;
    }
    
    /// <summary>
    /// Instanciate a child of the canvas.
    /// </summary>
    /// <param name="childToInstantiate">The GameObject to instanciate.</param>
    /// <param name="isActive">The initial state of the child.</param>
    public void InstantiateChild(GameObject childToInstantiate, bool isActive = true)
    {
        GameObject child = Instantiate(childToInstantiate, m_canvas.transform, false);
        child.SetActive(isActive);
    }
    
    /// <summary>
    /// Destroys a child GameObject of the canvas.
    /// </summary>
    /// <param name="childToDestroy">The GameObject to destroy.</param>
    public void DestroyChild(GameObject childToDestroy)
    {
        if (childToDestroy != null)
        {
            Destroy(childToDestroy);
        }
    }
    
    /// <summary>
    /// Sets the active state of a specified child GameObject.
    /// </summary>
    /// <param name="child">The child GameObject to modify.</param>
    /// <param name="isActive">The desired active state of the child GameObject.</param>
    public void SetEnabledOfChild(GameObject child, bool isActive)
    {
        if (child != null)
        {
            child.SetActive(isActive);
        }
    }
    
    /// <summary>
    /// Enables or disables a child GameObject of the canvas by its name.
    /// </summary>
    /// <param name="childName">The name of the child GameObject to modify.</param>
    /// <param name="isActive">The desired active state of the child GameObject.</param>
    public void SetEnabledOfChild(string childName, bool isActive)
    {
        foreach (Transform child in m_canvas.transform)
        {
            if (child.name == childName)
            {
                child.gameObject.SetActive(isActive);
                break;
            }
        }
    }
}
