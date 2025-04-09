using UnityEngine;
using UnityEngine.EventSystems;

public class PZL_RadioBtn : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    [SerializeField] float frotationSpeed = 5f;
    [SerializeField] float fmaxLeftRotation = -1800f;
    [SerializeField] float fmaxRightRotation = 720f;

    float currentRotationZ;
    Vector2 centerPoint;
    bool isDragging;

    private void Start()
    {
        currentRotationZ = transform.eulerAngles.z;
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
    }

    public bool pickAble()
    {
        if (Mathf.Abs(currentRotationZ) >= fmaxRightRotation - 50)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }

}
