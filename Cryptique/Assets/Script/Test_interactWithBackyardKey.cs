using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class InteractWithBackyardKey : OBJ_InteractOnDrop
{
    [Header("Interact with Backyard Key")]
    [SerializeField] private NavMeshObstacle m_navMeshObstacle;
    public override bool Interact()
    {
        GetComponent<PZL_Gate>().DoComplete();
        m_navMeshObstacle.enabled = false;
        return true;
    }

}
