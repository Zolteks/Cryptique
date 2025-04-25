using UnityEngine;

public class AutoPuzzleCamera : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;
    [SerializeField]
    private float padding = 1.1f;
    [SerializeField]
    private float minSize = 5f;
    [SerializeField]
    private bool onStarte = true;

    void Start()
    {
        if(onStarte)
            FitPuzzleInView();
    }

    public void FitPuzzleInView()
    {
        Bounds bounds = CalculateBounds();

        float screenAspect = (float)Screen.width / Screen.height;
        float targetRatio = bounds.size.x / bounds.size.y;

        float size;
        if (screenAspect >= targetRatio)
            size = bounds.size.y / 2f;
        else
            size = bounds.size.x / (2f * screenAspect);

        targetCamera.orthographicSize = Mathf.Max(size * padding, minSize);

        Vector3 center = bounds.center;
        targetCamera.transform.position = new Vector3(center.x, center.y, targetCamera.transform.position.z);
    }

    private Bounds CalculateBounds()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
            return new Bounds(transform.position, Vector3.one);

        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
            bounds.Encapsulate(r.bounds);

        return bounds;
    }
}
