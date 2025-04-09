using System.Collections;
using UnityEngine;

public class SwipeDetection : MonoBehaviour
{
    /* Singleton */
    private InputManager m_inputManager;
    
    /* Variables */
    [SerializeField]
    private float m_minSwipeDistance = 0.1f;
    [SerializeField]
    private float m_maxtime = 1f;

    private Vector2 m_startPosition;
    private float m_startTime = 0f;
    private Vector2 m_endPosition;
    private float m_endTime = 0f;

    /* Functions */
    private void Awake()
    {
        m_inputManager = InputManager.Instance;
    }
    
    private void OnEnable()
    {
        m_inputManager.OnStartTouch += SwipeStart;
        m_inputManager.OnEndTouch += SwipeEnd;
    }
    
    private void OnDisable()
    {
        m_inputManager.OnStartTouch -= SwipeStart;
        m_inputManager.OnEndTouch -= SwipeEnd;
    }
    
    private void SwipeStart(Vector2 position, float time)
    {
        m_startPosition = position;
        m_startTime = time;
    }
    
    private void SwipeEnd(Vector2 position, float time)
    {
        m_endPosition = position;
        m_endTime = time;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        //Debug.Log("Distance: " + Vector3.Distance(m_startPosition, m_endPosition));
        if (Vector3.Distance(m_startPosition, m_endPosition) >= m_minSwipeDistance &&
            (m_endTime - m_startTime) <= m_maxtime)
        {
            Debug.Log("Swipe Detected");
            Debug.DrawLine(m_startPosition, m_endPosition, Color.red, 5f);
        }
    }
}
