using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class DragAndDrop : Singleton<DragAndDrop>
{
    /* Singleton */
    private InputManager m_inputManager;
    
    /* Variables */
    [SerializeField]
    private Canvas m_canvas;
    [SerializeField]
    private GraphicRaycaster m_graphicRaycaster;

    private Camera m_mainCamera;
    private GameObject m_draggedObject;
    private GameObject m_ghostImageObject;
    private RectTransform m_ghostRect;
    private OBJ_Item m_selectedItem;
    
    Coroutine m_dragCoroutine;
    
    /* Functions */
    private void Awake()
    {
        m_inputManager = InputManager.Instance;
        m_mainCamera = Camera.main;
    }
    
    private void OnEnable()
    {
        m_inputManager.OnStartTouch += OnDragStart;
        m_inputManager.OnEndTouch += OnDragEnd;
    }
    
    private void OnDisable()
    {
        m_inputManager.OnStartTouch -= OnDragStart;
        m_inputManager.OnEndTouch -= OnDragEnd;
    }
    
    private void OnDragStart(Vector2 position, float time)
    {
        // Raycast UI
        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = position };
        List<RaycastResult> results = new List<RaycastResult>();
        m_graphicRaycaster.Raycast(pointerData, results);

        if (results.Count > 0)
        {
            GameObject hitUI = results[0].gameObject;
            Debug.Log(hitUI.name);
            Image img = hitUI.GetComponent<Image>();
            OBJ_DraggableItem draggableItem = hitUI.GetComponent<OBJ_DraggableItem>();

            if (img != null && draggableItem != null && draggableItem.enabled)
            {
                m_selectedItem = draggableItem.GetItem();
                m_draggedObject = hitUI;

                // Créer une ghost image
                m_ghostImageObject = new GameObject("GhostImage", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
                m_ghostImageObject.transform.SetParent(m_canvas.transform, false);

                m_ghostRect = m_ghostImageObject.GetComponent<RectTransform>();
                Image ghostImg = m_ghostImageObject.GetComponent<Image>();

                // Copier le sprite et les paramètres de l'original
                ghostImg.sprite = img.sprite;
                ghostImg.preserveAspect = true;
                ghostImg.raycastTarget = false; // Important : ne pas intercepter les raycasts

                // Tu peux personnaliser le visuel (alpha, ombre, effet visuel, etc.)
                Color c = img.color;
                c.a = 0.7f;
                ghostImg.color = c;

                // Ajuster la taille
                m_ghostRect.sizeDelta = img.rectTransform.sizeDelta;

                // Positionner directement la première fois
                UpdateGhostPosition(position);
                
                // Démarrer la coroutine de drag
                m_dragCoroutine = StartCoroutine(OnDrag());
            }
        }
        else Debug.Log("No UI hit");
    }
    
    private IEnumerator OnDrag()
    {
        while (m_draggedObject != null)
        {
            // Mettre à jour la position de l'image fantôme
            UpdateGhostPosition(m_inputManager.GetTouchPosition());
            yield return null;
        }
    }
    
    private void OnDragEnd(Vector2 position, float time)
    {
        if (m_ghostImageObject == null)
            return;
        
        Destroy(m_ghostImageObject); 
        m_ghostImageObject = null;
        
        if (m_dragCoroutine != null)
            StopCoroutine(m_dragCoroutine);

        m_draggedObject = null;

        GameObject objectToInteract = Utils.GetObjectUnderTouch(m_mainCamera, position);
        if (objectToInteract != null)
        {
            Debug.Log(objectToInteract.name);
            OBJ_InteractOnDrop objectInteract = objectToInteract.GetComponentInParent<OBJ_InteractOnDrop>();
            if(objectToInteract != null)
                objectInteract.UseItemOnDrop(m_selectedItem);
        }
        else Debug.Log("No Object to interact with");
    }
    
    private void UpdateGhostPosition(Vector2 screenPos)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_canvas.transform as RectTransform,
            screenPos,
            m_canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : m_mainCamera,
            out Vector2 localPos
        );

        m_ghostRect.anchoredPosition = localPos;
    }
}
