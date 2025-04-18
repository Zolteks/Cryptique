using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteBehaviour : OBJ_Interactable
{
    [SerializeField] PZL_Simon pzl;
    [SerializeField] int id;

    [NonSerialized] public Color m_initCol;

    private void Start()
    {
        m_initCol = GetComponent<Renderer>().material.GetColor("_EdgesColor");
    }

    public override bool Interact()
    {
        pzl.TriggerButton(id);
        return true;
    }
}
