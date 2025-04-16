using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class SGL_InputManager : Singleton<SGL_InputManager>
{
    #region Events
    public delegate void StartOnTouch(Vector2 position, float time);
    public event StartOnTouch OnStartTouch;
    public delegate void EndOnTouch(Vector2 position, float time);
    public event EndOnTouch OnEndTouch;
    public delegate void StartOnClick(Vector2 position, float time);
    public event StartOnClick OnClick;
    #endregion
    
    /* Variables */
    private TouchControl m_touchControl;
    private Camera m_mainCamera;
    [Header("Click Settings")]
    [SerializeField] private float m_maxHoldTime = 0.20f;
    private float m_touchStartTime;
    
    /* Functions */
    private void Awake()
    {
        m_touchControl = new TouchControl();
        if (m_touchControl == null)
            Debug.LogError("Failed to initialize TouchControl");
        m_mainCamera = Camera.main;
        if (m_mainCamera == null)
            Debug.LogError("Camera not found");
    }
    
    private void OnEnable()
    {
        m_touchControl.Enable();
    }
    
    private void OnDisable()
    {
        m_touchControl.Disable();
    }
    
    private void Start()
    {
        m_touchControl.Touch.TouchPress.started += ctx => StartTouch(ctx);
        m_touchControl.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }
    
    private void StartTouch(InputAction.CallbackContext context)
    {
        m_touchStartTime = (float)context.startTime;
        if (OnStartTouch != null)
            OnStartTouch(m_touchControl.Touch.TouchPosition.ReadValue<Vector2>(), m_touchStartTime);
    }
    
    private void EndTouch(InputAction.CallbackContext context)
    {
        float touchEndTime = (float)context.time;
        float holdDuration = touchEndTime - m_touchStartTime;

        if (OnEndTouch != null)
            OnEndTouch(m_touchControl.Touch.TouchPosition.ReadValue<Vector2>(), touchEndTime);

        // Vérifie si la durée est inférieure ou égale à m_maxHoldTime
        if (OnClick != null && m_maxHoldTime >= holdDuration)
            OnClick(m_touchControl.Touch.TouchPosition.ReadValue<Vector2>(), touchEndTime);
    }
    
    public Vector2 GetTouchPosition()
    {
        return m_touchControl.Touch.TouchPosition.ReadValue<Vector2>();
    }
}
