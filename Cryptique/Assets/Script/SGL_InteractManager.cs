using UnityEngine;
using UnityEngine.UI;

public class SGL_InteractManager : Singleton<SGL_InteractManager>
{
    /* Singleton */
    private static SGL_InputManager m_inputManager;
    
    /* Variables */
    [Header("Settings")]
    [SerializeField] private Camera m_camera;
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private GraphicRaycaster m_graphicRaycaster;

    private bool bEnabled;
    
    /* Functions */
    private void Awake()
    {
        m_inputManager = SGL_InputManager.Instance;
        if (m_inputManager == null)
            Debug.LogError("InputManager not found");
        m_camera = Camera.main;
        if (m_camera == null)
            Debug.LogError("Camera not found");
        m_canvas = UIManager.Instance.GetCanvas();
        if (m_canvas == null)
            Debug.LogError("Canvas not found");
        m_graphicRaycaster = UIManager.Instance.GetGraphicRaycaster();
        if (m_graphicRaycaster == null)
            Debug.LogError("GraphicRaycaster not found");
    }

    private void OnEnable()
    {
        EnableInteraction();
    }
    
    private void OnDisable()
    {
        DisableInteraction();
    }
    
    public void EnableInteraction()
    {
        if (bEnabled) return;

        bEnabled = true;
        m_inputManager.OnClick += OnInteract;
    }
    
    public void DisableInteraction()
    {
        if (false == bEnabled) return;

        bEnabled = false;
        m_inputManager.OnClick -= OnInteract;
    }

    public void ChangeCamera(Camera cam) => m_camera = cam;

    private void OnInteract(Vector2 pos, float time)
    {
        // Raycast UI
        if (m_graphicRaycaster != null && Utils.DetectHitWithUI(pos, m_graphicRaycaster))
            return;
        // Raycast 3D
        var arrowObject = Utils.GetArrowUnderTouch(m_camera, pos);
        var hitObject = Utils.GetObjectUnderTouch(m_camera, pos);
        if (hitObject == null && arrowObject != null)
        {
            arrowObject.GetComponent<OBJ_Interactable>().TriggerInteract();
            return;
        }
        if (hitObject == null)
            return;
        var interactableOnDrop = hitObject.GetComponentInParent<OBJ_InteractOnDrop>();
        if (interactableOnDrop != null && interactableOnDrop.CanInteract())
            return;
        var interactable = hitObject.GetComponentInParent<OBJ_Interactable>();
        if (interactable != null && interactable.CanInteract())
            interactable.TriggerInteract();
        else if (arrowObject)
            arrowObject.GetComponent<OBJ_Interactable>().TriggerInteract();
    }
}
