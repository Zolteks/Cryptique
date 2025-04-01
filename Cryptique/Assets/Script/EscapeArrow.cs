using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeArrow : MonoBehaviour
{
    [Tooltip("Let empty if the arrow doesn't lead to another region")]
    [SerializeField] string goToRegion;

    // TODO: To be replaced by and access trough game manager singleton or similar
    [SerializeField] Transform m_cameraAnchor;

    private void OnMouseDown()
    {
        if (goToRegion == "")
        {
            Vector3 flattenDir = transform.forward;
            flattenDir.y = 0;
            flattenDir.Normalize();

            m_cameraAnchor.position += flattenDir * 500;
        }
        else
        {
            SceneManager.LoadScene(goToRegion);
        }
    }
}
