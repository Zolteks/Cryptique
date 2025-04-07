using UnityEngine;

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
}
