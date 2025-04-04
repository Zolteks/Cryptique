using UnityEngine;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    #region Events
    public delegate void StartOnTouch(Vector2 position, float time);
    public event StartOnTouch OnStartTouch;
    public delegate void EndOnTouch(Vector2 position, float time);
    public event EndOnTouch OnEndTouch;
    #endregion

    /* Variables */
    private TouchControl m_touchControl;
    private Camera m_mainCamera;

    /* Functions */
    private void Awake()
    {
        m_touchControl = new TouchControl();
        m_mainCamera = Camera.main;
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
        //Debug.Log("StartTouch");
        if (OnStartTouch != null)
            OnStartTouch(GetTouchPosition(), (float)context.startTime);
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        //Debug.Log("EndTouch");
        if (OnEndTouch != null)
            OnEndTouch(GetTouchPosition(), (float)context.time);
    }

    public Vector2 GetTouchPosition()
    {
        return Utils.ScreenToWorld(m_mainCamera, m_touchControl.Touch.TouchPosition.ReadValue<Vector2>());
    }
}
