using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteBehaviour : OBJ_Interactable
{
    [SerializeField] PZL_Simon pzl;
    [SerializeField] int id;

    [NonSerialized] public Material m_initMat;

    private void Start()
    {
        m_initMat = GetComponent<MeshRenderer>().material;
    }

    public override bool Interact()
    {
        pzl.TriggerButton(id);
        return true;
    }
}
