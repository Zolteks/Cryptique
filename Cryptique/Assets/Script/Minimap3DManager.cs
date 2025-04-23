using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class PlacedTileData
{
    public GameObject tile;
    public int minimapLevel = 0;
}

public class Minimap3DManager : MonoBehaviour
{
    [Header("Original tiles")]
    public List<PlacedTileData> placedTiles = new List<PlacedTileData>();

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

    [Header("Player tracking")]
    public Transform player;
    private Vector3 lastPlayerPosition;
    private Quaternion lastMarkerRotation;

    private float unlockTile = 10f;
    public Color defaultTileColor = Color.white;

    private Dictionary<GameObject, GameObject> visitedMiniTiles = new Dictionary<GameObject, GameObject>();
    private Dictionary<GameObject, GameObject> miniTileMarkers = new Dictionary<GameObject, GameObject>();
    private GameObject playerMarker;
    private List<GameObject> activeLines = new List<GameObject>();

    public RawImage minimapRawImage;
    public RenderTexture minimapTexture;

    [System.Serializable]
    public class TileHighlight
    {
        public GameObject tile;
        public GameObject miniObject;
    }

    [Header("Map Objects in Tiles")]
    public List<TileHighlight> objectInTiles = new List<TileHighlight>();

    void Start()
    {
        SetupIsometricCameraView();
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
            }
        }
        RotatingObject();

    }


    void LateUpdate()
    {
        if (playerMarker != null)
        {
            Quaternion currentRotation = playerMarker.transform.rotation;
            float cameraY = Camera.main.transform.eulerAngles.y;
            currentRotation = Quaternion.Euler(0, -cameraY, 0);

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

        Bounds bounds = new Bounds(placedTiles[0].tile.transform.position, Vector3.zero);
        foreach (PlacedTileData data in placedTiles)
        {
            bounds.Encapsulate(data.tile.transform.position);
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

    void CreateMiniTile(GameObject tile)
    {
        PlacedTileData tileData = placedTiles.Find(t => t.tile == tile);
        if (tileData == null) return;

        Vector3 originalPos = tile.transform.position;
        Vector3 miniPos = minimapOrigin + (originalPos / scaleFactor);

        float levelYOffset = tileData.minimapLevel * 1f;
        miniPos.y += levelYOffset;

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

            int layer = Mathf.RoundToInt(Mathf.Log(minimapLayer.value, 2));
            marker.layer = layer;
            foreach (Transform child in marker.transform)
            {
                child.gameObject.layer = layer;
            }

            miniTileMarkers[tile] = marker;
        }
    }

    private void RotatingObject()
    {
        if (playerMarker == null) return;

        GameObject currentTile = null;
        float closestDist = Mathf.Infinity;

        foreach (var kvp in visitedMiniTiles)
        {
            float dist = Vector3.Distance(playerMarker.transform.position, kvp.Value.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                currentTile = kvp.Key;
            }
        }

        foreach (var kvp in miniTileMarkers)
        {
            GameObject tile = kvp.Key;
            GameObject miniObj = kvp.Value;

            if (tile == currentTile)
            {
                float floatHeight = 0.5f;
                float floatSpeed = 2f;
                float floatAmplitude = 0.1f;

                Vector3 basePos = playerMarker.transform.position + Vector3.up * floatHeight;
                float offsetY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
                miniObj.transform.position = basePos + Vector3.up * offsetY;

                float rotationSpeed = 50f;
                miniObj.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
            }
            else
            {
                if (visitedMiniTiles.TryGetValue(tile, out GameObject miniTile))
                {
                    Renderer rend = miniTile.GetComponentInChildren<Renderer>();
                    float yOffset = rend != null ? rend.bounds.extents.y + 0.2f : 0.3f;
                    Vector3 defaultPos = miniTile.transform.position + Vector3.up * yOffset;

                    miniObj.transform.position = defaultPos;
                    miniObj.transform.rotation = Quaternion.identity;
                }
            }
        }
    }


    public void UpdateMiniMapPlayerPosition()
    {
        GameObject closestTile = null;
        float minDist = Mathf.Infinity;

        foreach (PlacedTileData data in placedTiles)
        {
            GameObject tile = data.tile;
            float dist = Vector3.Distance(player.position, tile.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                closestTile = tile;
            }
        }

        if (closestTile == null) return;

        PlacedTileData currentData = placedTiles.Find(t => t.tile == closestTile);
        int currentLevel = currentData != null ? currentData.minimapLevel : 0;

        foreach (var kvp in new Dictionary<GameObject, GameObject>(visitedMiniTiles))
        {
            PlacedTileData tileData = placedTiles.Find(t => t.tile == kvp.Key);
            if (tileData != null && tileData.minimapLevel != currentLevel)
            {
                Destroy(kvp.Value);
                visitedMiniTiles.Remove(kvp.Key);
                if (miniTileMarkers.ContainsKey(kvp.Key))
                {
                    Destroy(miniTileMarkers[kvp.Key]);
                    miniTileMarkers.Remove(kvp.Key);
                }
            }
        }

        if (!visitedMiniTiles.ContainsKey(closestTile) && currentData.minimapLevel == currentLevel)
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

        foreach (GameObject line in activeLines)
        {
            Destroy(line);
        }
        activeLines.Clear();

        foreach (Transform child in closestTile.transform)
        {
            if (!child.gameObject.activeSelf || !child.CompareTag("EscapeArrow"))
                continue;

            Vector3 direction = child.forward;
            float targetDistance = 500f;
            Vector3 targetPoint = child.position + direction * targetDistance;

            GameObject targetTile = null;
            float closestDistance = Mathf.Infinity;

            foreach (PlacedTileData candidateData in placedTiles)
            {
                GameObject candidate = candidateData.tile;
                float dist = Vector3.Distance(candidate.transform.position, targetPoint);
                if (dist < 100f && dist < closestDistance)
                {
                    closestDistance = dist;
                    targetTile = candidate;
                }
            }

            if (targetTile != null)
            {
                PlacedTileData targetData = placedTiles.Find(t => t.tile == targetTile);

                Vector3 endPoint;

                bool targetVisited = visitedMiniTiles.TryGetValue(targetTile, out GameObject targetMiniTile);
                if (targetVisited)
                {
                    endPoint = targetMiniTile.transform.position;
                }
                else
                {
                    float yOffset = targetData != null ? targetData.minimapLevel * 1f : 0f;
                    endPoint = minimapOrigin + (targetTile.transform.position / scaleFactor);
                    endPoint.y += yOffset;
                }

                GameObject line = Instantiate(linePrefab);
                LineRenderer lr = line.GetComponent<LineRenderer>();

                if (lr != null)
                {
                    Vector3 startPoint = visitedMiniTiles.ContainsKey(closestTile)
                        ? visitedMiniTiles[closestTile].transform.position
                        : minimapOrigin + (closestTile.transform.position / scaleFactor);

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

        UpdateMinimapRawImage();
        CameraRotation();
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
}
