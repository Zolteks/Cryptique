using System.Collections;
using UnityEngine;

public class PZL_MovableWoodObject : MonoBehaviour
{
    [SerializeField] private PZL_SortingGame pzlGame;

    private Vector3 vOffset;
    private float fZCoord;
    private bool bIsDragging = false;
    private bool isPlaced = false;
    private string currentZone = null;

    void OnMouseDown()
    {
        if (isPlaced)
            return; // Empêche de déplacer un bâton déjà placé correctement

        fZCoord = pzlGame.cam.WorldToScreenPoint(transform.position).z;
        vOffset = transform.position - GetMouseWorldPos();
        bIsDragging = true;
    }

    void OnMouseDrag()
    {
        if (bIsDragging && !isPlaced)
        {
            transform.position = GetMouseWorldPos() + vOffset;
        }
    }

    void OnMouseUp()
    {
        bIsDragging = false;
        if (currentZone != null)
        {
            GameObject currentZoneFind = GameObject.Find(currentZone);
            int zoneID = ExtractID(currentZoneFind.name);
            int stickID = ExtractID(gameObject.name);

            // Positionner le bâton dans la zone cible
            Vector3 targetPosition = currentZoneFind.transform.position + new Vector3(0.75f, 0f, -0.3f);
            print("target pos : " + targetPosition);
            StartCoroutine(SmoothMovement(targetPosition, 4f));

            if (zoneID == stickID)
            {
                // Placement correct
                isPlaced = true;
                pzlGame.UpdateEtatPlacement(zoneID - 1, true);
            }
            else
            {
                // Placement incorrect, le bâton reste déplaçable
                Debug.Log("Placement incorrect. Le bâton peut être déplacé à nouveau.");
            }
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = fZCoord;
        return pzlGame.cam.ScreenToWorldPoint(mousePoint);
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.name);
        if (bIsDragging && !isPlaced)
        {
            currentZone = other.name;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print(other.name);
        if (currentZone == other.name)
        {
            currentZone = null;
        }
    }

    private IEnumerator SmoothMovement(Vector3 targetPosition, float speed)
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;
    }

    private int ExtractID(string name)
    {
        // Supposons que l'ID est le dernier caractère du nom
        if (int.TryParse(name.Substring(name.Length - 1), out int id))
        {
            return id;
        }
        return -1; // Valeur par défaut en cas d'échec
    }
}
