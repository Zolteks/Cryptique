using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PC_PlayerController : MonoBehaviour
{
    [Header("Player Movement")]
    [SerializeField] [Range(3f, 6f)] private float m_moveSpeed = 4.5f;
    [Header("Animations")]
    private bool m_isInputActive = true;
    [SerializeField] private string m_idleStateName = "charlie_idle";
    [SerializeField] private string m_interactionParameter = "Interaction";
    [SerializeField] private string m_SpeedParameter = "MotionSpeed";
    
    private Camera m_camera;
    private InputManager m_inputManager;
    private NavMeshAgent m_agent;
    private Animator m_animator;
    
    private Coroutine m_coroutineInteraction;
    private Coroutine m_coroutineWaitForInteraction;

    public bool GetInputActive() => m_isInputActive;
    
    private void Awake()
    {
        m_camera = Camera.main;
        if (m_camera == null)
        {
            Debug.LogError("Camera not found");
            return;
        }
        m_inputManager = InputManager.Instance;
        if (m_inputManager == null)
        {
            Debug.LogError("InputManager not found");
            return;
        }
        m_agent = GetComponentInChildren<NavMeshAgent>();
        if (m_agent == null)
        {
            Debug.LogError("NavMeshAgent not found");
            return;
        }
        m_agent.speed = m_moveSpeed;
        m_animator = GetComponentInChildren<Animator>();
        if (m_animator == null)
            Debug.LogError("Animator not found");
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
        m_isInputActive = false;
    }
    
    public void EnableInput()
    {
        m_inputManager.OnStartTouch += CalculatePoint;
        m_isInputActive = true;
    }

    private void LateUpdate()
    {
        m_animator?.SetFloat(m_SpeedParameter, m_agent.velocity.magnitude);
        Vector3 cameraPosition = m_camera.transform.position;
        cameraPosition.y = transform.position.y;
        transform.GetChild(0).transform.LookAt(cameraPosition);
        transform.GetChild(0).transform.Rotate(0f, 180f, 0f);
    }
    
    public void SetDesinationPoint(Vector3 destination)
    {
        NavMesh.SamplePosition(destination, out NavMeshHit hit, 50.0f, NavMesh.AllAreas);
        m_agent.SetDestination(hit.position);
    }
    
    public void MoveAndInteract(Vector3 destination)
    {
        // Désactiver les inputs
        if (m_isInputActive)
            DisableInput();
        // Lancer le mouvement
        m_agent.ResetPath();
        SetDesinationPoint(destination);
        m_coroutineWaitForInteraction = StartCoroutine(CoroutineWaitForInteraction());
    }

    private IEnumerator CoroutineWaitForInteraction()
    {
        // Attendre que l'agent atteigne sa destination
        while (m_agent.pathPending || m_agent.remainingDistance > m_agent.stoppingDistance)
        {
            yield return null;
        }
        // Lancer l'interaction une fois arrivé
        m_coroutineInteraction = StartCoroutine(LauchInteraction());
    }

    public IEnumerator LauchInteraction()
    {
        m_animator?.SetTrigger(m_interactionParameter);
        while(!m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_idleStateName))
        {
            yield return null;
        }
        Debug.Log("Interaction finished");
        // Réactiver les inputs
        if (!m_isInputActive)
            EnableInput();
    }
}
