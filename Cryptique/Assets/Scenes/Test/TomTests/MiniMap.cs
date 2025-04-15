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
    public bool useIsometricView = true;

    [Header("Tracking du joueur")]
    public Transform player;
    public float highlightRadius = 10f;
    public Color playerTileColor = Color.red;
    public Color surroundingTileColor = Color.yellow;
    public Color defaultTileColor = Color.white;

    private List<GameObject> miniTiles = new List<GameObject>();
    private Dictionary<GameObject, GameObject> tileToMiniTile = new Dictionary<GameObject, GameObject>();

    void Start()
    {
        CreateMiniMapTiles();

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

    void CreateMiniMapTiles()
    {
        if (miniTilePrefab == null || placedTiles.Count == 0)
        {
            Debug.LogWarning("Aucun prefab ou tile original assigné !");
            return;
        }

        foreach (GameObject tile in placedTiles)
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

            miniTiles.Add(miniTile);
            tileToMiniTile[tile] = miniTile;
        }
    }

    void SetupIsometricCameraView()
    {
        if (minimapCamera == null || miniTiles.Count == 0) return;

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
        if (minimapCamera == null || miniTiles.Count == 0) return;

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

    void UpdateMiniTileHighlights()
    {
        // Trouver la tile la plus proche du joueur
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

        // Réinitialiser toutes les couleurs
        foreach (var miniTile in tileToMiniTile.Values)
        {
            Renderer rend = miniTile.GetComponent<Renderer>();
            if (rend != null)
            {
                rend.material.color = defaultTileColor;
            }
        }

        if (closestTile != null)
        {
            if (tileToMiniTile.TryGetValue(closestTile, out GameObject playerMiniTile))
            {
                Renderer rend = playerMiniTile.GetComponent<Renderer>();
                if (rend != null)
                {
                    rend.material.color = playerTileColor;
                }
            }

            // Mettre en surbrillance les tiles autour
            foreach (GameObject tile in placedTiles)
            {
                if (tile == closestTile) continue;

                float dist = Vector3.Distance(tile.transform.position, closestTile.transform.position);
                if (dist < highlightRadius)
                {
                    if (tileToMiniTile.TryGetValue(tile, out GameObject miniTile))
                    {
                        Renderer rend = miniTile.GetComponent<Renderer>();
                        if (rend != null)
                        {
                            rend.material.color = surroundingTileColor;
                        }
                    }
                }
            }
        }
    }
}
