using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Collider))]
public class OBJ_RotateAble : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    [Header("Rotation Settings")]
    [Range(2f, 6f)] public float rotationSpeed = 2f;
    public bool invertRotation = false;

    [Header("Axes")]
    public bool allowX = true;
    public bool allowY = true;
    public bool allowZ = false;

    private Vector2 previousPosition;
    private Camera mainCamera;
    private float touchTime;

    void Awake()
    {
        if (Camera.main.GetComponent<PhysicsRaycaster>() == null)
            Camera.main.gameObject.AddComponent<PhysicsRaycaster>();
    }

    void Start()
    {
        mainCamera = Camera.main;
        if (EventSystem.current == null)
            new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        previousPosition = eventData.position;
        touchTime = Time.time;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Calcul du delta
        Vector2 delta = eventData.position - previousPosition;
        float sensitivity = Mathf.Clamp(1.5f - (Time.time - touchTime), 0.5f, 1.5f);

        // Rotation en fonction des axes autorisés
        Vector3 rotation = Vector3.zero;
        if (allowY) rotation.x = delta.y * rotationSpeed * sensitivity;
        if (allowX) rotation.y = -delta.x * rotationSpeed * sensitivity;
        if (allowZ) rotation.z = allowX ? delta.x * rotationSpeed * 0.5f * sensitivity : 0;

        if (invertRotation) rotation *= -1;

        // Application directe
        transform.Rotate(rotation, Space.World);
        previousPosition = eventData.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, GetComponent<Collider>().bounds.size);
    }
}