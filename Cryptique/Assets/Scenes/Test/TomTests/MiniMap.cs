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
    public float cameraDistance = 10f;
    public Vector3 cameraOffsetDirection = new Vector3(-1f, 2f, -1f);
    public bool useIsometricView = true;

    [Header("Player tracking")]
    public Transform player;
    private Vector3 lastPlayerPosition;

    public float highlightRadius = 10f;
    public Color defaultTileColor = Color.white;

    private Dictionary<GameObject, GameObject> visitedMiniTiles = new Dictionary<GameObject, GameObject>();
    private GameObject playerMarker;
    private List<GameObject> activeLines = new List<GameObject>();

    public RawImage minimapRawImage;
    public RenderTexture minimapTexture;

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
    }

    void UpdateMiniMapPlayerPosition()
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

        // Create mini-tile if not visited yet
        if (!visitedMiniTiles.ContainsKey(closestTile))
        {
            CreateMiniTile(closestTile);
        }

        // Create the player marker if it does not exist
        if (playerMarker == null && playerMarkerPrefab != null)
        {
            playerMarker = Instantiate(playerMarkerPrefab);
            playerMarker.transform.localScale = Vector3.one * 0.3f;
            playerMarker.layer = Mathf.RoundToInt(Mathf.Log(minimapLayer.value, 2));
        }

        // Position the player marker above the mini-tile
        if (visitedMiniTiles.TryGetValue(closestTile, out GameObject miniTile))
        {
            playerMarker.transform.position = miniTile.transform.position + Vector3.up * 0.5f;
        }

        // Remove old lines
        foreach (GameObject line in activeLines)
        {
            Destroy(line);
        }
        activeLines.Clear();

        // Create lines to nearby tiles
        foreach (GameObject tile in placedTiles)
        {
            if (tile == closestTile) continue;

            float dist = Vector3.Distance(tile.transform.position, closestTile.transform.position);
            if (dist < highlightRadius)
            {
                if (!visitedMiniTiles.ContainsKey(tile))
                {
                    CreateMiniTile(tile);
                }

                if (visitedMiniTiles.TryGetValue(tile, out GameObject otherMiniTile))
                {
                    GameObject line = Instantiate(linePrefab);
                    LineRenderer lr = line.GetComponent<LineRenderer>();

                    if (lr != null)
                    {
                        lr.positionCount = 2;
                        lr.SetPosition(0, miniTile.transform.position);
                        lr.SetPosition(1, otherMiniTile.transform.position);
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
}
