using UnityEngine;
using UnityEngine.AI;

public class PC_PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] private float m_moveSpeed = 5f;
    
    private Camera m_camera;
    private InputManager m_inputManager;
    private NavMeshAgent m_agent;
    
    private void Awake()
    {
        m_camera = Camera.main;
        m_inputManager = InputManager.Instance;
        m_agent = GetComponentInChildren<NavMeshAgent>();
        if (m_agent != null)
        {
            m_agent.speed = m_moveSpeed;
        }
    }
    
    private void OnEnable()
    {
        m_inputManager.OnStartTouch += CalculatePoint;
    }
    
    private void OnDisable()
    {
        m_inputManager.OnStartTouch -= CalculatePoint;
    }

    private void CalculatePoint(Vector2 touchPoint, float time)
    {
        // Convertir la position de l'écran en position du monde
        Vector3 worldPoint = Utils.ScreenToWorld(m_camera, touchPoint);
        // Raycast pour trouver le point de destination
        Ray ray = m_camera.ScreenPointToRay(touchPoint);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Définir le point de destination sur le NavMeshAgent
            SetDesinationPoint(hit.point);
        }
    }
    
    public void DisableInput()
    {
        m_inputManager.OnStartTouch -= CalculatePoint;
    }
    
    public void EnableInput()
    {
        m_inputManager.OnStartTouch += CalculatePoint;
    }
    
    public void SetDesinationPoint(Vector3 destination)
    {
        if (m_agent != null)
        {
            m_agent.SetDestination(destination);
        }
    }
}
