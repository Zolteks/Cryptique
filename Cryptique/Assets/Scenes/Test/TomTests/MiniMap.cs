using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Minimap3DManager : MonoBehaviour
{
    [Header("Tiles for map")]
    public List<GameObject> placedTiles;

    [Header("Mini-map Settings")]
    public GameObject miniTilePrefab;
    public float scaleFactor = 100f;
    public Vector3 minimapOrigin = Vector3.zero;
    public LayerMask minimapLayer;

    [Header("Camera minimap")]
    public Camera minimapCamera;
    public float cameraDistance = 10f;
    public bool useIsometricView = true;

    private List<GameObject> miniTiles = new List<GameObject>();

    void Start()
    {
        CreateMiniMapTiles();

        if (useIsometricView)
            SetupIsometricCameraView();
        else
            SetupTopDownCameraView();
    }

    void CreateMiniMapTiles()
    {
        if (miniTilePrefab == null || placedTiles.Count == 0)
        {
            Debug.LogWarning(" no prefab assigned ");
            return;
        }

        foreach (GameObject tile in placedTiles)
        {
            Vector3 originalPos = tile.transform.position;
            Vector3 miniPos = minimapOrigin + (originalPos / scaleFactor);

            GameObject miniTile = Instantiate(miniTilePrefab, miniPos, Quaternion.identity);
            miniTile.transform.localScale = Vector3.one * 0.5f;
            miniTile.layer = Mathf.RoundToInt(Mathf.Log(minimapLayer.value, 2));

            miniTiles.Add(miniTile);
        }
    }

    void SetupIsometricCameraView()
    {
        if (minimapCamera == null || miniTiles.Count == 0)
        {
            Debug.LogWarning(" No camera assigned ");
            return;
        }

        // Bounds calculated to find the center
        Bounds bounds = new Bounds(miniTiles[0].transform.position, Vector3.zero);
        foreach (GameObject tile in miniTiles)
        {
            bounds.Encapsulate(tile.transform.position);
        }

        Vector3 center = bounds.center;

        Vector3 offsetDirection = new Vector3(-1f, 1f, -1f).normalized;
        Vector3 cameraPos = center + offsetDirection * cameraDistance;

        minimapCamera.transform.position = cameraPos;
        minimapCamera.transform.LookAt(center);
        minimapCamera.cullingMask = minimapLayer;

        minimapCamera.orthographic = false;
        minimapCamera.fieldOfView = 45f;
    }

    void SetupTopDownCameraView()
    {
        if (minimapCamera == null || miniTiles.Count == 0)
            return;

        Bounds bounds = new Bounds(miniTiles[0].transform.position, Vector3.zero);
        foreach (GameObject tile in miniTiles)
        {
            bounds.Encapsulate(tile.transform.position);
        }

        Vector3 center = bounds.center;
        minimapCamera.transform.position = new Vector3(center.x, center.y + cameraDistance, center.z);
        minimapCamera.transform.rotation = Quaternion.Euler(90f, 180f, 180f);

        minimapCamera.orthographic = false;
        minimapCamera.fieldOfView = 60f;
        minimapCamera.cullingMask = minimapLayer;
    }
}
