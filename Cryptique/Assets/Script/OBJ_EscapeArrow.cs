using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OBJ_EscapeArrow : OBJ_Interactable
{
    [Tooltip("Let empty if the arrow doesn't lead to another region")]
    [SerializeField] string goToRegion;

    /*[SerializeField]*/ float fEaseMaxSpeed = 1200;
    /*[SerializeField]*/ float fEaseThreshold = 60;

    bool m_isBusy = false;

    Transform m_cameraAnchor;

    private void Start()
    {
        m_cameraAnchor = GameManager.GetInstance().GetCamera();
    }

    public override bool Interact()
    {
        if (false == CanInteract())
            return false;

        if (goToRegion == "")
        {
            Vector3 flattenDir = transform.forward;
            flattenDir.y = 0;
            flattenDir.Normalize();

            StartCoroutine(CoroutineEaseBetweenTiles(m_cameraAnchor.position, m_cameraAnchor.position + flattenDir * 500, flattenDir));
            //m_cameraAnchor.position += flattenDir * 500;
        }
        else
        {
            SaveAndLoadScene.Excute(goToRegion);
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
        Interact();
    }
}
