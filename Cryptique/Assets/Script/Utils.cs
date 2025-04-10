using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Utils : MonoBehaviour
{
    public static Vector3 ScreenToWorld(Camera camera, Vector3 position)
    {
        position.z = camera.nearClipPlane;
        return camera.ScreenToWorldPoint(position);
    }
    
    public static GameObject GetObjectUnderTouch(Camera camera, Vector3 mousePosition)
    {
        Ray ray = camera.ScreenPointToRay(mousePosition);
        return Physics.Raycast(ray, out RaycastHit hit) ? hit.collider.gameObject : null;
    }

    public static bool DetectHitWithUI(Vector2 touchPosition, GraphicRaycaster raycaster)
    {
        // Raycast UI
        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = touchPosition };
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        if (results.Count > 0)
            return true;
        return false;
    }
}
