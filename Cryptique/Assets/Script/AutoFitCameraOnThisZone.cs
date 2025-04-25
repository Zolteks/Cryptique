using UnityEngine;

[RequireComponent(typeof(Collider))]
public class AutoFitCameraOnThisZone : MonoBehaviour
{
    [SerializeField] private float padding = 1.2f;
    [SerializeField] private float minDistance = 5f;
    [SerializeField] private float maxDistance = 50f;
    [SerializeField] private float cameraHeight = 10f; // Pour la vue isométrique
    [SerializeField] private Camera targetCamera;

    void Awake()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    public void FitCameraToThisZone()
    {
        Bounds bounds = new Bounds(transform.position, Vector3.zero);
        foreach (Renderer r in GetComponentsInChildren<Renderer>())
        {
            bounds.Encapsulate(r.bounds);
        }

        float size = Mathf.Max(bounds.size.x, bounds.size.z) * padding;

        float fov = targetCamera.fieldOfView;
        float aspect = targetCamera.aspect;

        float distance = size / (2f * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad));
        distance = Mathf.Clamp(distance, minDistance, maxDistance);

        // Positionner la caméra comme si elle était en vue isométrique (à 45° par ex)
        Vector3 center = bounds.center;
        Vector3 offset = Quaternion.Euler(45f, 45f, 0f) * Vector3.back * distance;

        targetCamera.transform.position = center + offset + Vector3.up * cameraHeight;
        targetCamera.transform.LookAt(center);
    }
}
