using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class PZL_Crank : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] private float frotationSpeed = 5f;
    [SerializeField] private GameObject lanterne;
    [SerializeField] private float fmaxLeftRotation = -1800f;
    [SerializeField] private float fmaxRightRotation = 720f;

    [SerializeField] private float fminScale = 0.5f;
    [SerializeField] private float fmaxScale = 2f;

    private float currentRotationZ;
    private Vector2 centerPoint;
    private bool isDragging;
    private Vector3 initialScale;

    private void Start()
    {
        currentRotationZ = transform.eulerAngles.z;
        initialScale = lanterne.transform.localScale;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        
        isDragging = true;
        centerPoint = RectTransformUtility.WorldToScreenPoint(null, transform.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;

        Vector2 direction = eventData.position - centerPoint;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float deltaRotation = Mathf.DeltaAngle(currentRotationZ, targetAngle);
        currentRotationZ = Mathf.Clamp(currentRotationZ + deltaRotation * frotationSpeed * Time.deltaTime, fmaxLeftRotation, fmaxRightRotation);

        transform.rotation = Quaternion.Euler(0, 0, currentRotationZ);
        UpdateLanterneScale();
    }

    public bool pickAble()
    {
        if (Mathf.Abs(currentRotationZ) >= fmaxRightRotation-50)
        {
            Debug.Log("Suck my flacid C***");
            return true;
        }
        else
        {
            Debug.Log("I don't work ! Suck my flacid C***");
            return false;
        }
    }

    private void UpdateLanterneScale()
    {
        // Since camera placed in front it seems left and right are inverted.
        // Scaling Left is scaled up x2 and Right is scaled down /2
        float normalizedRotation = Mathf.InverseLerp(fmaxLeftRotation, fmaxRightRotation, currentRotationZ);

        float currentScale = Mathf.Lerp(fmaxScale, fminScale, normalizedRotation);

        lanterne.transform.localScale = initialScale * currentScale;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

}
