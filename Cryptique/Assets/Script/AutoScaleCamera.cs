using System.Collections;
using UnityEngine;

public class AutoScaleCamera : MonoBehaviour
{
    [SerializeField]
    private Camera targetCamera;
    [SerializeField]
    private float padding = 1.1f;
    [SerializeField]
    private float maxSize = 10f;
    [SerializeField]
    private float minSize = 5f;
    [SerializeField]
    private bool fitOnStart = false;

    private void Start()
    {
        if(fitOnStart)
        {
            FitView();
        }
    }


    public void FitView()
    {
        //wait the time travel of transition
        StartCoroutine(WaitTransition());
       
        //targetCamera.orthographicSize = 4f; // Set a fixed orthographic size for testing

        //Vector3 center = bounds.center;
        //targetCamera.transform.position = new Vector3(center.x, center.y, targetCamera.transform.position.z);
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

    private IEnumerator WaitTransition()
    {
        yield return new WaitForSeconds(0.5f);
        Bounds bounds = CalculateBounds();

        float screenAspect = (float)Screen.width / Screen.height;
        float targetRatio = bounds.size.x / bounds.size.y;

        float size;
        if (screenAspect >= targetRatio)
            size = bounds.size.y / 2f;
        else
            size = bounds.size.x / (2f * screenAspect);

        targetCamera.orthographicSize = Mathf.Clamp(size * padding, minSize, maxSize);
    }
}

