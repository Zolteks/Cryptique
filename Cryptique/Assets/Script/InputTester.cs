using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTester : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
        }
    }

    private void OnClick()
    {
        Debug.Log("Clicked");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.Log("Raycast hit: " + hit.collider.name);

            if (hit.collider.GetComponentInParent<OBJ_Interactable>().Interact())
                Debug.Log("Interacted with " + hit.collider.name);

        }
    }
}
