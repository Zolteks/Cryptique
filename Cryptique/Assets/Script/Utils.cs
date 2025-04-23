using System.Collections.Generic;
using System.Linq;
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

    public static GameObject GetArrowUnderTouch(Camera camera, Vector3 mousePosition)
    {
        var ray = camera.ScreenPointToRay(mousePosition);
        return (from hit in Physics.RaycastAll(ray) where hit.collider.GetComponent<IN_EscapeArrow>() || hit.collider.GetComponent<IN_DecoyArrow>() select hit.collider.gameObject).FirstOrDefault();
    }
    
    public static GameObject GetObjectUnderTouch(Camera camera, Vector3 mousePosition)
    {
        var ray = camera.ScreenPointToRay(mousePosition);
        return Physics.Raycast(ray, out var hit, 100, ~(4096 + 8192)) ? hit.collider.gameObject : null;
    }

    public static bool DetectHitWithUI(Vector2 touchPosition, GraphicRaycaster raycaster)
    {
        // Raycast UI
        var pointerData = new PointerEventData(EventSystem.current) { position = touchPosition };
        var results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);
        return results.Count > 0;
    }
}
