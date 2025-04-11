using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InteractManager : Singleton<InteractManager>
{
    private static InputManager m_inputManager;
    
    private Camera m_camera;
    [SerializeField]
    private Canvas m_canvas;
    [SerializeField]
    private GraphicRaycaster m_graphicRaycaster;
    
    private void Awake()
    {
        m_inputManager = InputManager.Instance;
        m_camera = Camera.main;
        m_canvas = UIManager.Instance.GetCanvas();
        m_graphicRaycaster = UIManager.Instance.GetGraphicRaycaster();
    }

    private void OnEnable()
    {
        m_inputManager.OnStartTouch += OnInteract;
    }
    
    private void OnDisable()
    {
        m_inputManager.OnStartTouch -= OnInteract;
    }

    private void OnInteract(Vector2 pos, float time)
    {
        // Raycast UI
        if (m_graphicRaycaster != null && Utils.DetectHitWithUI(pos, m_graphicRaycaster))
            return;
        else Debug.Log("UI hit !!!");
            // Raycast 3D
            GameObject hitObject = Utils.GetObjectUnderTouch(m_camera, pos);
        if (hitObject != null)
        {
            Debug.Log("Interacted with : " + hitObject.name);
            var interactableOnDrop = hitObject.GetComponentInParent<OBJ_InteractOnDrop>();
            if (interactableOnDrop != null && interactableOnDrop.CanInteract())
                return;
            var interactable = hitObject.GetComponentInParent<OBJ_Interactable>();
            if (interactable != null && interactable.CanInteract())
            {
                interactable.Interact();
                Debug.Log("Interacted with: " + hitObject.name);
            }
        }
        else Debug.Log("No object hit");
    }
}
