using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_MovableWoodObject : MonoBehaviour
{
    [SerializeField] private PZL_SortingGame pzlGame;

    private Vector3 vOffset;
    private float fZCoord;
    private bool bIsDragging = false;

    void OnMouseDown()
    {
        fZCoord = pzlGame.cam.WorldToScreenPoint(transform.position).z;
        vOffset = new Vector3(transform.position.x, transform.position.y, transform.position.z) - GetMouseWorldPos();
        bIsDragging = true;
    }

    void OnMouseDrag()
    {
        if (bIsDragging)
        {
            transform.position = GetMouseWorldPos() + vOffset;
        }
    }

    private void OnMouseUp()
    {
        bIsDragging = false;
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = fZCoord;
        return pzlGame.cam.ScreenToWorldPoint(mousePoint);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!bIsDragging && !pzlGame.GetisAllPlaced())
        {
            int cLastCharOnCollide = (int)Char.GetNumericValue(other.name[name.Length - 1]);
            int cLastCharGameObject = (int)Char.GetNumericValue(gameObject.name[name.Length - 1]);

            if (cLastCharOnCollide == cLastCharGameObject)
            {
                StartCoroutine(SmoothMovement(other.transform.position, 4));
                pzlGame.UpdateEtatPlacement(cLastCharOnCollide - 1, true);
            }
        }
    }

    private IEnumerator SmoothMovement(Vector3 positionCible, float vitesse)
    {
        while (Vector3.Distance(transform.position, positionCible) > 0.01f)
        {
            Vector3 newPosCible = new Vector3(positionCible.x + 0.75f, positionCible.y, positionCible.z - 0.3f);
            transform.position = Vector3.MoveTowards(transform.position, newPosCible, vitesse * Time.deltaTime);
            yield return null; // Attendre le prochain frame
        }
        gameObject.transform.position = positionCible;
    }
}
