using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class IN_EscapeArrow : OBJ_Interactable
{
    /* Variables */
    [Header("Arrow Settings")]
    [Tooltip("Let empty if the arrow doesn't lead to another region")]
    [SerializeField] private string m_goToRegion;
    [Tooltip("Let empty if the player don't need to be teleported")]
    [SerializeField] private GameObject m_teleportPoint;
    
    /*[SerializeField]*/ private float m_easeMaxSpeed = 1200;
    /*[SerializeField]*/ private float m_easeThreshold = 60;
    
    private static bool m_isBusy = false;
    private Transform m_cameraAnchor;
    [SerializeField] private PC_PlayerController m_playerController;
    [SerializeField] private UnityEvent tilesToResize;

    private SaveSystemManager m_SaveSystemManager;
    private GameProgressionManager m_gameProgressionManager;

    /* Functions */
    private void Awake()
    {
        m_playerController = PC_PlayerController.Instance;
        if (m_playerController == null)
            Debug.LogError("PlayerController not found");
    }
    
    private void Start()
    {
        m_cameraAnchor = GameManager.GetInstance().GetCamera();
        if (m_cameraAnchor == null)
            Debug.LogError("Camera anchor not found");
        if (gameObject.activeSelf)
        {
            Minimap3DManager minimap = FindFirstObjectByType<Minimap3DManager>();
            if (minimap != null)
            {
                minimap.UpdateMiniMapPlayerPosition();
            }
        }
    }

    public override void TriggerInteract()
    {
        if (m_isBusy)
            return;

        base.TriggerInteract();

        if (m_teleportPoint)
            m_playerController.MoveToTile(m_teleportPoint.transform.position);
        else
            m_playerController.MoveTo();
    }


    public override bool Interact()
    {
        if (string.IsNullOrEmpty(m_goToRegion))
        {
            Vector3 flattenDir = transform.forward;
            flattenDir.y = 0;
            flattenDir.Normalize();

            StartCoroutine(CoroutineEaseBetweenTiles(m_cameraAnchor.position, m_cameraAnchor.position + flattenDir * 500, flattenDir));
            //m_cameraAnchor.position += flattenDir * 500;
        }
        else
        {
            SceneManager.LoadScene(m_goToRegion);

            if(m_SaveSystemManager == null)
                m_SaveSystemManager = SaveSystemManager.Instance;
            if (m_gameProgressionManager == null)
                m_gameProgressionManager = GameProgressionManager.Instance;

            foreach(RegionData region in m_gameProgressionManager.GetRegions())
            {
                if (region.GetName() == m_goToRegion)
                {
                    m_SaveSystemManager.GetGameData().progression.currentRegion = region.GetName();
                    
                }
            }


        }

        return true;
    }
    
    public void SetArrowActive(bool active)
    {
        bool wasActive = gameObject.activeSelf;

        gameObject.SetActive(active);

        if (!wasActive && active)
        {
            Minimap3DManager minimap = Object.FindFirstObjectByType<Minimap3DManager>();
            if (minimap != null)
            {
                minimap.UpdateMiniMapPlayerPosition();
            }
        }
    }
    
    /* Coroutine */
    private IEnumerator CoroutineEaseBetweenTiles(Vector3 start, Vector3 end, Vector3 dir)
    {
        m_isBusy = true;

        while ((m_cameraAnchor.position - end).magnitude >= .1f)
        {
            float startRatio = Mathf.Min((m_cameraAnchor.position - start).magnitude, m_easeThreshold) / m_easeThreshold;
            float endRatio = Mathf.Min((m_cameraAnchor.position - end).magnitude, m_easeThreshold) / m_easeThreshold;
            float smallestRatio = Mathf.Min(startRatio, endRatio);

            var value = dir * Mathf.Lerp(1f, m_easeMaxSpeed, smallestRatio) * Time.deltaTime;
            m_cameraAnchor.position += value;

            tilesToResize.Invoke();

            yield return new WaitForFixedUpdate();
        }

        m_isBusy = false;
    }
}
