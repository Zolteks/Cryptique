using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OBJ_EscapeArrow : OBJ_Interactable
{
    [Tooltip("Let empty if the arrow doesn't lead to another region")]
    [SerializeField] string goToRegion;

    // TODO: To be replaced by and access trough game manager singleton or similar
    [SerializeField] Transform m_cameraAnchor;

    public override bool Interact()
    {
        if (false == CanInteract())
            return false;

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
        return true;
    }

    // Is that definitive? Unsure
    private void OnMouseDown()
    {
        Interact();
    }
}
