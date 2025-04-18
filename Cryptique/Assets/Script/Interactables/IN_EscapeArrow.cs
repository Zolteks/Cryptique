using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IN_EscapeArrow : OBJ_Interactable
{
    [Tooltip("Let empty if the arrow doesn't lead to another region")]
    [SerializeField] string goToRegion;

    /*[SerializeField]*/ float fEaseMaxSpeed = 1200;
    /*[SerializeField]*/ float fEaseThreshold = 60;

    static bool m_isBusy = false;

    Transform m_cameraAnchor;

    private void Start()
    {
        m_cameraAnchor = GameManager.GetInstance().GetCamera();

        if (gameObject.activeSelf)
        {
            Minimap3DManager minimap = FindFirstObjectByType<Minimap3DManager>();
            if (minimap != null)
            {
                minimap.UpdateMiniMapPlayerPosition();
            }
        }
    }

    public override bool Interact()
    {
        if (false == CanInteract() || m_isBusy)
            return false;

        if (string.IsNullOrEmpty(goToRegion))
        {
            Vector3 flattenDir = transform.forward;
            flattenDir.y = 0;
            flattenDir.Normalize();

            StartCoroutine(CoroutineEaseBetweenTiles(m_cameraAnchor.position, m_cameraAnchor.position + flattenDir * 500, flattenDir));
            //m_cameraAnchor.position += flattenDir * 500;
        }
        else
        {
            SceneManager.LoadScene(goToRegion);
        }
        return true;
    }

    IEnumerator CoroutineEaseBetweenTiles(Vector3 start, Vector3 end, Vector3 dir)
    {
        m_isBusy = true;

        while ((m_cameraAnchor.position - end).magnitude >= .1f)
        {
            float startRatio = Mathf.Min((m_cameraAnchor.position - start).magnitude, fEaseThreshold) / fEaseThreshold;
            float endRatio = Mathf.Min((m_cameraAnchor.position - end).magnitude, fEaseThreshold) / fEaseThreshold;
            float smallestRatio = Mathf.Min(startRatio, endRatio);

            var value = dir * Mathf.Lerp(1f, fEaseMaxSpeed, smallestRatio) * Time.deltaTime;
            m_cameraAnchor.position += value;

            yield return new WaitForFixedUpdate();
        }

        m_isBusy = false;
    }

    // Is that definitive? Unsure
    private void OnMouseDown()
    {
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

        Interact();
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

}
