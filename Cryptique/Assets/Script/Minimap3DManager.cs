using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Minimap3DManager : MonoBehaviour
{
    [Header("Original tiles")]
    public List<GameObject> placedTiles;

    [Header("Minimap settings")]
    public GameObject miniTilePrefab;
    public GameObject playerMarkerPrefab;
    public GameObject linePrefab;
    public float scaleFactor = 100f;
    public Vector3 minimapOrigin = Vector3.zero;
    public LayerMask minimapLayer;

    [Header("Minimap camera")]
    public Camera minimapCamera;
    public float cameraDistance = 1000f;
    public Vector3 cameraOffsetDirection = new Vector3(-1f, 2f, -1f);
    public bool useIsometricView = true;

    [Header("Player tracking")]
    public Transform player;
    private Vector3 lastPlayerPosition;
    private Quaternion lastMarkerRotation;


    private float unlockTile = 10f;
    public Color defaultTileColor = Color.white;

    private Dictionary<GameObject, GameObject> visitedMiniTiles = new Dictionary<GameObject, GameObject>();
    private Dictionary<GameObject, GameObject> miniTileMarkers = new Dictionary<GameObject, GameObject>(); // Clone of the objects to put in the map
    private GameObject playerMarker;
    private List<GameObject> activeLines = new List<GameObject>();

    public RawImage minimapRawImage;
    public RenderTexture minimapTexture;

    [System.Serializable]
    public class TileHighlight
    {
        public GameObject tile;
        public GameObject miniObject; // The objects to put in the map
    }

    [Header("Map Objects in Tiles")]
    public List<TileHighlight> objectInTiles = new List<TileHighlight>();


    void Start()
    {
        if (useIsometricView)
            SetupIsometricCameraView();
        else
            SetupTopDownCameraView();
        UpdateMiniMapPlayerPosition();
        UpdateMinimapRawImage();
    }

    void Update()
    {
        if (player != null)
        {
            if (player.position != lastPlayerPosition)
            {
                lastPlayerPosition = player.position;

                UpdateMiniMapPlayerPosition();
                UpdateMinimapRawImage();
            }
        }
    }

    void LateUpdate()
    {
        if (playerMarker != null)
        {
            Quaternion currentRotation = playerMarker.transform.rotation;

            if (currentRotation != lastMarkerRotation)
            {
                lastMarkerRotation = currentRotation;
                CameraRotation();
            }
        }
    }

    void SetupIsometricCameraView()
    {
        if (minimapCamera == null || placedTiles.Count == 0) return;

        Bounds bounds = new Bounds(placedTiles[0].transform.position, Vector3.zero);
        foreach (GameObject tile in placedTiles)
        {
            bounds.Encapsulate(tile.transform.position);
        }

        Vector3 center = bounds.center;
        Vector3 offsetDirection = cameraOffsetDirection.normalized;
        Vector3 cameraPos = minimapOrigin + (center / scaleFactor) + offsetDirection * cameraDistance;

        minimapCamera.transform.position = cameraPos;
        minimapCamera.transform.LookAt(minimapOrigin + (center / scaleFactor));
        minimapCamera.cullingMask = minimapLayer;
        minimapCamera.orthographic = false;
        minimapCamera.fieldOfView = 45f;
    }

    void SetupTopDownCameraView()
    {
        if (minimapCamera == null || placedTiles.Count == 0) return;

        Bounds bounds = new Bounds(placedTiles[0].transform.position, Vector3.zero);
        foreach (GameObject tile in placedTiles)
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

    void CreateMiniTile(GameObject tile)
    {
        Vector3 originalPos = tile.transform.position;
        Vector3 miniPos = minimapOrigin + (originalPos / scaleFactor);

        GameObject miniTile = Instantiate(miniTilePrefab, miniPos, Quaternion.identity);
        miniTile.transform.localScale = Vector3.one * 0.5f;
        miniTile.layer = Mathf.RoundToInt(Mathf.Log(minimapLayer.value, 2));

        Renderer rend = miniTile.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = defaultTileColor;
        }

        visitedMiniTiles[tile] = miniTile;
        TileHighlight highlight = objectInTiles.Find(h => h.tile == tile);
        if (highlight != null && highlight.miniObject != null)
        {
            Renderer miniRenderer = miniTile.GetComponentInChildren<Renderer>();

            float yOffset = miniRenderer != null ? miniRenderer.bounds.extents.y + 0.2f : 0.3f;

            Vector3 markerPos = miniTile.transform.position + Vector3.up * yOffset;

            GameObject marker = Instantiate(highlight.miniObject, markerPos, Quaternion.identity);
            marker.transform.SetParent(miniTile.transform);
            marker.transform.localScale = Vector3.one * 0.4f;

            int layer = Mathf.RoundToInt(Mathf.Log(minimapLayer.value, 2));
            marker.layer = layer;

            foreach (Transform child in marker.transform)
            {
                child.gameObject.layer = layer;
            }

            miniTileMarkers[tile] = marker;
        }
    }


    public void UpdateMiniMapPlayerPosition()
    {
        GameObject closestTile = null;
        float minDist = Mathf.Infinity;

        foreach (GameObject tile in placedTiles)
        {
            float dist = Vector3.Distance(player.position, tile.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestTile = tile;
            }
        }

        if (closestTile == null) return;

        if (!visitedMiniTiles.ContainsKey(closestTile))
        {
            CreateMiniTile(closestTile);
        }

        if (playerMarker == null && playerMarkerPrefab != null)
        {
            playerMarker = Instantiate(playerMarkerPrefab);
            playerMarker.transform.localScale = Vector3.one * 0.3f;
            playerMarker.layer = Mathf.RoundToInt(Mathf.Log(minimapLayer.value, 2));
        }

        if (visitedMiniTiles.TryGetValue(closestTile, out GameObject miniTile))
        {
            playerMarker.transform.position = miniTile.transform.position + Vector3.up * 0.5f;
        }

        RepositionCamera();

        foreach (TileHighlight highlight in objectInTiles)
        {
            if (highlight.tile == closestTile)
            {
                if (highlight.tile != null && miniTileMarkers.ContainsKey(highlight.tile))
                {
                    GameObject marker = miniTileMarkers[highlight.tile];

                    if (marker != null)
                    {
                        marker.SetActive(false);
                    }
                }
            }
            else
            {
                if (highlight.tile != null && miniTileMarkers.ContainsKey(highlight.tile))
                {
                    GameObject marker = miniTileMarkers[highlight.tile];

                    if (marker != null)
                    {
                        marker.SetActive(true);
                    }
                }
            }
        }

        foreach (GameObject line in activeLines)
        {
            Destroy(line);
        }
        activeLines.Clear();

        foreach (Transform child in closestTile.transform)
        {
            if (!child.gameObject.activeSelf) continue;

            if (child.CompareTag("EscapeArrow"))
            {
                Vector3 direction = child.forward;
                float targetDistance = 500f;
                Vector3 targetPoint = child.position + direction * targetDistance;

                GameObject targetTile = null;
                float closestDistance = Mathf.Infinity;

                foreach (GameObject candidate in placedTiles)
                {
                    float dist = Vector3.Distance(candidate.transform.position, targetPoint);
                    if (dist < 100f && dist < closestDistance)
                    {
                        closestDistance = dist;
                        targetTile = candidate;
                    }
                }

                if (targetTile != null)
                {
                    Vector3 endPoint;

                    if (visitedMiniTiles.TryGetValue(targetTile, out GameObject targetMiniTile))
                    {
                        endPoint = targetMiniTile.transform.position;
                    }
                    else
                    {
                        endPoint = minimapOrigin + (targetTile.transform.position / scaleFactor);
                    }

                    GameObject line = Instantiate(linePrefab);
                    LineRenderer lr = line.GetComponent<LineRenderer>();

                    if (lr != null)
                    {
                        Vector3 startPoint = miniTile.transform.position;

                        Vector3 dir = (endPoint - startPoint).normalized;

                        float offset = 0.25f;

                        Vector3 adjustedStart = startPoint + dir * offset;
                        Vector3 adjustedEnd = endPoint - dir * offset;

                        lr.positionCount = 2;
                        lr.SetPosition(0, adjustedStart);
                        lr.SetPosition(1, adjustedEnd);
                        lr.startWidth = lr.endWidth = 0.2f;
                    }

                    line.layer = Mathf.RoundToInt(Mathf.Log(minimapLayer.value, 2));
                    activeLines.Add(line);
                }
            }
        }
    }


    void UpdateMinimapRawImage()
    {
        if (minimapRawImage != null && minimapTexture != null)
        {
            minimapRawImage.texture = minimapTexture;
        }
    }

    void CameraRotation()
    {
        if (playerMarker != null && minimapCamera != null && Camera.main != null)
        {
            Quaternion mainCameraRotation = Camera.main.transform.rotation;

            minimapCamera.transform.rotation = mainCameraRotation;

            Vector3 offsetDir = Quaternion.Euler(0f, Camera.main.transform.eulerAngles.y, 0f) * cameraOffsetDirection.normalized;

            minimapCamera.transform.position = playerMarker.transform.position + offsetDir * cameraDistance;

            minimapCamera.transform.LookAt(playerMarker.transform.position);
        }
    }

    void RepositionCamera()
    {
        if (playerMarker != null && minimapCamera != null)
        {
            float angleY = playerMarker.transform.eulerAngles.y;

            Vector3 offsetDir = Quaternion.Euler(0f, angleY, 0f) * cameraOffsetDirection.normalized;

            minimapCamera.transform.position = playerMarker.transform.position + offsetDir * cameraDistance;

            minimapCamera.transform.LookAt(playerMarker.transform.position);
        }
    }

}
