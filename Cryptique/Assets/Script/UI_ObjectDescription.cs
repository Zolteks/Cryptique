using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ObjectDescription : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_itemName;
    [SerializeField] private TextMeshProUGUI m_itemDescription;
    [SerializeField] private RawImage rawImage;


    private Camera itemCamera;
    private GameObject previewInstance;
    private GameObject pivot;


    // Rotation

    private Vector2 lastTouchPosition;
    private bool isDragging = false;
    [SerializeField] private float rotationSpeed = 0.2f;


#if UNITY_EDITOR || UNITY_STANDALONE
    [SerializeField] private OBJ_Item item;

    private void Start()
    {
        ShowObjectDescription(item);
        Show3DObject(item);
    }
#endif

    private void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseRotation(); // pour le debug PC
#else
    HandleTouchRotation(); // mobile
#endif
    }


    public void ShowObjectDescription(OBJ_Item item)
    {
        m_itemName.text = item.GetName();
        m_itemDescription.text = item.GetDescription();
    }



    private void Show3DObject(OBJ_Item item)
    {
        if (itemCamera != null) Destroy(itemCamera.gameObject);
        if (previewInstance != null) Destroy(previewInstance);

        GameObject camObj = new GameObject("ItemCamera");
        itemCamera = camObj.AddComponent<Camera>();
        itemCamera.clearFlags = CameraClearFlags.SolidColor;
        itemCamera.backgroundColor = new Color(0, 0, 0, 0); // Transparent
        itemCamera.cullingMask = LayerMask.GetMask("InspectableObject");
        itemCamera.orthographic = false;
        itemCamera.fieldOfView = 30f;

        RenderTexture renderTexture = new RenderTexture(512, 512, 16);
        itemCamera.targetTexture = renderTexture;

        pivot = new GameObject("ItemPivot");
        pivot.transform.position = new Vector3(5000,5000,5000);

        previewInstance = Instantiate(item.GetPrefab(), pivot.transform);
        SetLayerRecursively(previewInstance, LayerMask.NameToLayer("InspectableObject"));
        previewInstance.transform.localPosition = Vector3.zero;

        itemCamera.transform.position = pivot.transform.position + new Vector3(0, 0, -10f);
        itemCamera.transform.LookAt(pivot.transform);

        Light light = new GameObject("ItemLight").AddComponent<Light>();
        light.type = LightType.Directional;
        light.transform.rotation = Quaternion.Euler(50, -30, 0);
        light.transform.SetParent(camObj.transform);

        rawImage.texture = renderTexture;
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
            SetLayerRecursively(child.gameObject, layer);
    }



    // Rotation

    private void HandleMouseRotation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastTouchPosition = Input.mousePosition;
            isDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
            isDragging = false;

        if (isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPosition;
            pivot.transform.Rotate(Vector3.up, -delta.x * rotationSpeed, Space.World);
            pivot.transform.Rotate(Vector3.forward, delta.y * rotationSpeed, Space.Self);
            
            lastTouchPosition = Input.mousePosition;
        }
    }

    private void OnEnable()
    {
        InputManager.Instance.OnStartTouch += HandleStartTouch;
        InputManager.Instance.OnEndTouch += HandleEndTouch;
    }
    private void OnDisable()
    {
        if (InputManager.Instance == null) return;
        InputManager.Instance.OnStartTouch -= HandleStartTouch;
        InputManager.Instance.OnEndTouch -= HandleEndTouch;
    }

    private void HandleStartTouch(Vector2 pos, float time)
    {
        lastTouchPosition = pos;
        isDragging = true;
    }

    private void HandleEndTouch(Vector2 pos, float time)
    {
        isDragging = false;
    }

    private void HandleTouchRotation()
    {
        if (isDragging)
        {
            Vector2 currentTouchPosition = InputManager.Instance.GetTouchPosition();
            Vector2 delta = currentTouchPosition - lastTouchPosition;

            pivot.transform.Rotate(Vector3.up, -delta.x * rotationSpeed, Space.World);
            pivot.transform.Rotate(Vector3.forward, delta.y * rotationSpeed, Space.Self);

            lastTouchPosition = currentTouchPosition;
        }
    }




}
