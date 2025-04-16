using UnityEngine;
using UnityEngine.EventSystems;

public class PZL_Crank : MonoBehaviour
{
    [SerializeField] private GameObject m_lantern;
    [SerializeField] private float rotationSpeed = 0.5f;

    [SerializeField] private float fmaxLeftRotation = -1800f;
    [SerializeField] private float fmaxRightRotation = 720f;
    [SerializeField] private float fminScale = 0.5f;
    [SerializeField] private float fmaxScale = 2f;

    private float currentRotationZ;
    private Vector3 initialScale;
    private Vector3 lastMousePosition;
    private OBJ_Collectable m_LanternCollectable;

    void Start()
    {
        // Appliquer la rotation fixe de 180° sur l'axe X
        Vector3 initialRotation = transform.eulerAngles;
        initialRotation.x = 180f;
        transform.eulerAngles = initialRotation;
        initialScale = m_lantern.transform.localScale;
        m_LanternCollectable = m_lantern.GetComponent<OBJ_Collectable>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            float angleDelta = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

            float deltaRotation = Mathf.DeltaAngle(currentRotationZ, angleDelta);
            currentRotationZ = Mathf.Clamp(currentRotationZ + deltaRotation * rotationSpeed * Time.deltaTime, fmaxLeftRotation, fmaxRightRotation);
            transform.rotation = Quaternion.Euler(0, 0, currentRotationZ);

            UpdateLanterneScale();
        }
    }

    private void UpdateLanterneScale()
    {
        if (!m_LanternCollectable) return;

        // Since camera placed in front it seems left and right are inverted.
        // Scaling Left is scaled up x2 and Right is scaled down /2
        float normalizedRotation = Mathf.InverseLerp(fmaxLeftRotation, fmaxRightRotation, currentRotationZ);

        float currentScale = Mathf.Lerp(fmaxScale, fminScale, normalizedRotation);

        m_lantern.transform.localScale = initialScale * currentScale;

        if (Mathf.Abs(currentRotationZ) >= fmaxRightRotation)
        {
            m_LanternCollectable.SetCanInteract(true);
        }
        else
        {
            m_LanternCollectable.SetCanInteract(false);
        }
    }

}
