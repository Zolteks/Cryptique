using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Minimap3DManager : MonoBehaviour
{
    [Header("Tiles originales")]
    public List<GameObject> placedTiles;

    [Header("Mini-map Settings")]
    public GameObject miniTilePrefab;
    public float scaleFactor = 100f;
    public Vector3 minimapOrigin = Vector3.zero;
    public LayerMask minimapLayer;

    [Header("Caméra minimap")]
    public Camera minimapCamera;
    public float cameraDistance = 10f;
    public Vector3 cameraOffsetDirection = new Vector3(-1f, 2f, -1f); // modifiable depuis l'inspecteur
    public bool useIsometricView = true;

    [Header("Tracking du joueur")]
    public Transform player;
    public float highlightRadius = 10f;
    public Color playerTileColor = Color.red;
    public Color surroundingTileColor = Color.yellow;
    public Color defaultTileColor = Color.white;

    private Dictionary<GameObject, GameObject> visitedMiniTiles = new Dictionary<GameObject, GameObject>();

    void Start()
    {
        if (useIsometricView)
            SetupIsometricCameraView();
        else
            SetupTopDownCameraView();
    }

    void Update()
    {
        if (player != null)
        {
            UpdateMiniTileHighlights();
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

    void UpdateMiniTileHighlights()
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

        //  Créer la mini-tile du joueur si pas encore visitée
        if (!visitedMiniTiles.ContainsKey(closestTile))
        {
            CreateMiniTile(closestTile);
        }

        //  Réinitialiser les couleurs
        foreach (var miniTile in visitedMiniTiles.Values)
        {
            Renderer rend = miniTile.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = defaultTileColor;
            }
        }

        // Colorer la case actuelle du joueur
        if (visitedMiniTiles.TryGetValue(closestTile, out GameObject playerMiniTile))
        {
            playerMiniTile.GetComponent<Renderer>().material.color = playerTileColor;
        }

        //  Tiles autour
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

                if (visitedMiniTiles.TryGetValue(tile, out GameObject miniTile))
                {
                    miniTile.GetComponent<Renderer>().material.color = surroundingTileColor;
                }
            }
        }
    }
}
