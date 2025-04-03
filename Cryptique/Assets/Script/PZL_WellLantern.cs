using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PZL_WellLantern : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private float rotationSpeed = 5f;
    private float previousAngle;
    private bool isDragging = false;
    private Vector2 centerPoint;

    [SerializeField] private GameObject lantern;

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

    private float GetAngle(Vector2 screenPosition)
    {
        Vector2 direction = screenPosition - centerPoint;
        return Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        centerPoint = RectTransformUtility.WorldToScreenPoint(null, transform.position);
        previousAngle = GetAngle(eventData.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 direction = eventData.position - centerPoint;

        // DeadZone du Drag
        if (direction.magnitude < 20f) return;

        float currentAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float deltaAngle = Mathf.DeltaAngle(transform.eulerAngles.z, currentAngle);

        transform.rotation = Quaternion.Euler(0, 0, currentAngle);
    }

    public void DragLimit()
    {

    }

    public void WindItUp()
    {

    }

}
