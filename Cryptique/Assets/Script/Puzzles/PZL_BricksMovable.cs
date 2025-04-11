using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_BricksMovable : MonoBehaviour
{
    public enum Mouvement { Vertical, Horizontal }

    [SerializeField] private Mouvement modeMouvement;
    

    private Vector3 offset;
    private Camera mainCamera;
    private bool isDragging = false;

    private MeshRenderer mrMeshRenderer;

    void Start()
    {
        mainCamera = Camera.main;
        mrMeshRenderer = GetComponent<MeshRenderer>();

    }

    void OnMouseDown()
    {
        // Calculer l'offset entre la position de la souris et la position de l'objet
        Vector3 screenPoint = mainCamera.WorldToScreenPoint(transform.position);
        offset = transform.position - mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        isDragging = true;
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, mainCamera.WorldToScreenPoint(transform.position).z);
            Vector3 currentPosition = mainCamera.ScreenToWorldPoint(currentScreenPoint) + offset;

            // Limiter le déplacement à l'axe autorisé
            switch (modeMouvement)
            {
                case Mouvement.Vertical:
                    currentPosition.x = transform.position.x; // Empêcher le déplacement sur l'axe X
                    break;
                case Mouvement.Horizontal:
                    currentPosition.y = transform.position.y; // Empêcher le déplacement sur l'axe Y
                    break;
            }

            // Calculer la direction du mouvement
            Vector3 direction = currentPosition - transform.position;
            float distance = direction.magnitude;

            // Définir la taille de la boîte pour le BoxCast
            Vector3 boxSize = mrMeshRenderer.bounds.size;
            Vector3 halfExtents = boxSize / 2f;

            // Effectuer le BoxCast pour vérifier les obstacles
            RaycastHit hit;
            if (Physics.BoxCast(transform.position, halfExtents, direction.normalized, out hit, Quaternion.identity, distance))
            {
                return;
            }

            // Déplacer l'objet
            transform.position = currentPosition;
        }
    }


    void OnMouseUp()
    {
        isDragging = false;
    }

    
}
