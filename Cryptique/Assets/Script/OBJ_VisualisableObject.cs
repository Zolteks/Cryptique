﻿using UnityEngine;

public class OBJ_VisualisableObject : OBJ_Interactable
{

    [SerializeField] private GameObject uiVisualisation;
    [SerializeField] private OBJ_Item item;

    public override bool Interact()
    {
        uiVisualisation.SetActive(true);
        if (uiVisualisation.activeSelf)
        {
            UI_ObjectDescription uiObjectDescription = uiVisualisation.GetComponent<UI_ObjectDescription>();
            if (uiObjectDescription != null)
            {
                uiObjectDescription.Show(item);
            }
        }

        return true;
    }
    public override void TriggerInteract()
    {
        base.TriggerInteract();
    }
}

