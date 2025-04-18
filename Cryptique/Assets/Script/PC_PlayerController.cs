using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class PC_PlayerController : Singleton<PC_PlayerController>
{
    public enum PlayerRotation
    {
        Right = 0,
        Left = 180
    }
    
    #region Events
    public delegate void EndOfMoveCallback();
    public event EndOfMoveCallback OnMoveCallback;
    public delegate void EndOfInteractionCallback();
    public event EndOfInteractionCallback OnInteractionCallback;
    #endregion
    
    [Header("Movement")]
    [SerializeField] [Range(1f, 10f)] private float m_moveSpeed = 3f;
    [Header("Interaction")]
    [SerializeField] [Range(1f, 15f)] private float m_interactionDistance = 8f;
    [Header("Rotation")]
    [SerializeField] private PlayerRotation m_playerRotation = PlayerRotation.Left;
    [Header("Animations")]
    [SerializeField] private string m_idleStateName = "charlie_idle";
    [SerializeField] private string m_SpeedParameter = "MotionSpeed";
    [SerializeField] private string m_interactionTriggerParameter = "InteractionTrigger";
    [SerializeField] private string m_isInteractingParameter = "IsInteracting";
    private bool m_isInputActive = true;
    private Vector3 m_newtilePosition = Vector3.zero;
    
    private Camera m_camera;
    private SGL_InputManager m_inputManager;
    private SGL_InteractManager m_interactManager;
    private NavMeshAgent m_agent;
    private Animator m_animator;
    
    private Coroutine m_coroutineWaitFor;
    private Coroutine m_coroutineInteraction;
    
    public bool GetInputActive() => m_isInputActive;
    
    private void Awake()
    {
        m_camera = Camera.main;
        if (m_camera == null)
            Debug.LogError("Camera not found");
        m_inputManager = SGL_InputManager.Instance;
        if (m_inputManager == null)
            Debug.LogError("InputManager not found");
        m_interactManager = SGL_InteractManager.Instance;
        if (m_interactManager == null)
            Debug.LogError("InteractManager not found");
        m_agent = GetComponent<NavMeshAgent>();
        if (m_agent == null)
            Debug.LogError("NavMeshAgent not found");
        m_animator = GetComponentInChildren<Animator>();
        if (m_animator == null)
            Debug.LogError("Animator not found");
        m_agent.speed = m_moveSpeed;
    }
    
    private void OnEnable()
    {
        m_inputManager.OnClick += CalculatePoint;
    }
    
    private void OnDisable()
    {
        m_inputManager.OnClick -= CalculatePoint;
    }
    
    private void LateUpdate()
    {
        m_animator?.SetFloat(m_SpeedParameter, m_agent.velocity.magnitude);
        Vector3 cameraPosition = m_camera.transform.position;
        cameraPosition.y = transform.position.y;
        transform.GetChild(0).transform.LookAt(cameraPosition);
        transform.GetChild(0).transform.Rotate(0f, (float)m_playerRotation, 0f);
    }
    
    private void CalculatePoint(Vector2 touchPoint, float time)
    {
        // Raycast pour trouver le point de destination
        Ray ray = m_camera.ScreenPointToRay(touchPoint);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Définir le point de destination sur le NavMeshAgent
            SetDesination(hit.point);
        }
    }
    
    public void EnableInput()
    {
        Debug.Log("Enable input");
        m_inputManager.OnClick += CalculatePoint;
        m_interactManager.EnableInteraction();
        m_isInputActive = true;
    }
    
    public void DisableInput()
    {
        Debug.Log("Disable input");
        m_inputManager.OnClick -= CalculatePoint;
        m_interactManager.DisableInteraction();
        m_isInputActive = false;
    }
    
    private void FlipSprite()
    {
        m_playerRotation = m_playerRotation == PlayerRotation.Left ? PlayerRotation.Right : PlayerRotation.Left;
    }
    
    private void CheckFlipSprite(Vector3 destination)
    {
        Transform childTransform = transform.GetChild(0).transform;
        Vector3 direction = (destination - childTransform.position).normalized;
        if (m_playerRotation == PlayerRotation.Left && Vector3.Dot(childTransform.right, direction) > 0f)
        {
            //Debug.Log("Droite");
            FlipSprite();
        }
        else if (m_playerRotation == PlayerRotation.Right && Vector3.Dot(-childTransform.right, direction) < 0f)
        {
            //Debug.Log("Gauche");
            FlipSprite();
        }
    }
    
    private void SetDesination(Vector3 destination)
    {
        m_agent.ResetPath();
        NavMesh.SamplePosition(destination, out NavMeshHit hit, m_interactionDistance, NavMesh.AllAreas);
        Debug.Log("Agent go to : " + hit.position);
        m_agent.SetDestination(hit.position);
        CheckFlipSprite(hit.position);
    }

    public void MoveTo()
    {
        // Désactiver les inputs
        if (m_isInputActive)
            DisableInput();
        else return;
        m_coroutineWaitFor = StartCoroutine(CoroutineWaitFor());
    }
    
    public void MoveToTile(Vector3 newTilePosition)
    {
        // Désactiver les inputs
        if (m_isInputActive)
            DisableInput();
        else return;
        m_newtilePosition = newTilePosition;
        m_coroutineWaitFor = StartCoroutine(CoroutineWaitFor());
        OnMoveCallback += TeleportToTile;
    }
    
    private void TeleportToTile()
    {
        m_agent.ResetPath();
        NavMesh.SamplePosition(m_newtilePosition, out NavMeshHit hit, m_interactionDistance, NavMesh.AllAreas);
        Debug.Log("Agent teleport to : " + hit.position);
        m_agent.Warp(hit.position);
        if (!m_agent.isOnNavMesh)
            Debug.LogWarning("NavMesh not found");
        CheckFlipSprite(hit.position);
        OnMoveCallback -= TeleportToTile;
    }
    
    public void MoveForInteraction()
    {
        // Désactiver les inputs
        if (m_isInputActive)
            DisableInput();
        else return;
        m_animator.SetBool(m_isInteractingParameter, true);
        m_coroutineWaitFor = StartCoroutine(CoroutineWaitFor());
        OnMoveCallback += LaunchInteraction;
    }
    
    private void LaunchInteraction()
    {
        m_coroutineInteraction = StartCoroutine(CoroutineInteraction());
        OnMoveCallback -= LaunchInteraction;
    }
    
    /* Coroutines */
    private IEnumerator CoroutineWaitFor()
    {
        yield return new WaitForEndOfFrame();
        // Verifier si l'agent a atteint sa destination
        if ((!m_agent.pathPending && !m_agent.hasPath) && Vector3.Distance(gameObject.transform.position, m_agent.destination) > m_interactionDistance)
        {
            Debug.LogError("Object too far for interaction");
            EnableInput();
            yield break;
        }
        // Attendre que l'agent atteigne sa destination
        while (m_agent.pathPending || m_agent.velocity != Vector3.zero || m_agent.remainingDistance > m_agent.stoppingDistance)
        {
            yield return null;
        }
        // Lancer l'interaction une fois arrivé
        yield return new WaitForEndOfFrame();
        if (!m_isInputActive)
            EnableInput();
        OnMoveCallback?.Invoke();
        StopCoroutine(m_coroutineWaitFor);
    }
    
    private IEnumerator CoroutineInteraction()
    {
        //yield return new WaitForEndOfFrame();
        m_animator?.SetTrigger(m_interactionTriggerParameter);
        while(!m_animator.GetCurrentAnimatorStateInfo(0).IsName(m_idleStateName))
        {
            yield return null;
        }
        Debug.Log("Player interaction finished");
        // Réactiver les inputs
        yield return new WaitForEndOfFrame();
        if (!m_isInputActive)
            EnableInput();
        m_animator.SetBool(m_isInteractingParameter, false);
        OnInteractionCallback?.Invoke();
        StopCoroutine(m_coroutineInteraction);
    }
}
