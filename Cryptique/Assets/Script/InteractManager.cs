using UnityEngine;

public class InteractManager : Singleton<InteractManager>
{
    private static InputManager m_inputManager;
    
    private Camera m_camera;
    
    private void Awake()
    {
        m_inputManager = InputManager.Instance;
        m_camera = Camera.main;
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
        GameObject hitObject = Utils.GetObjectUnderTouch(m_camera, pos);
        if (hitObject != null)
        {
            OBJ_Interactable interactable = hitObject.GetComponentInParent<OBJ_Interactable>();
            if (interactable != null)
            {
                interactable.Interact();
                Debug.Log("Interacted with: " + hitObject.name);
            }
        }
    }
}
