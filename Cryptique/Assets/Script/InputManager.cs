using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    public delegate void StartOnTouch(Vector2 position, float time);
    public event StartOnTouch OnStartTouch;
    public delegate void EndOnTouch(Vector2 position, float time);
    public event EndOnTouch OnEndTouch;
    
    private TouchControl m_touchControl;
    
    private void Awake()
    {
        m_touchControl = new TouchControl();
    }

    private void OnEnable()
    {
        m_touchControl.Enable();
        EnhancedTouchSupport.Enable();

        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown += FingerDown;
    }
    
    private void OnDisable()
    {
        m_touchControl.Disable();
        EnhancedTouchSupport.Disable();
        
        UnityEngine.InputSystem.EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    private void Start()
    {
        m_touchControl.Touch.TouchPress.started += ctx => StartTouch(ctx);
        m_touchControl.Touch.TouchPress.canceled += ctx => EndTouch(ctx);
    }
    
    private void StartTouch(InputAction.CallbackContext context)
    {
        if (OnStartTouch != null)
            OnStartTouch(m_touchControl.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.startTime);
    }
    
    private void EndTouch(InputAction.CallbackContext context)
    {
        if (OnEndTouch != null)
            OnEndTouch(m_touchControl.Touch.TouchPosition.ReadValue<Vector2>(), (float)context.time);
    }

    private void FingerDown(Finger finger)
    {
        if (OnStartTouch != null)
            OnStartTouch(finger.screenPosition, Time.time);
    }

    private void Update()
    {
        Debug.Log(UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches);
        foreach (UnityEngine.InputSystem.EnhancedTouch.Touch touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            Debug.Log(touch.phase == UnityEngine.InputSystem.TouchPhase.Began);
        }
    }
}
