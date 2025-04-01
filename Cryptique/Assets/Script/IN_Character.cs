using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IN_Character : MonoBehaviour
{
    [SerializeField] string m_sName;
    [SerializeField] List<Transform> m_crossingPoints;

    NavMeshAgent m_navMeshAgent;
    int m_iPointIndex = 0;
    


    private void Start()
    {
        if(false == TryGetComponent<NavMeshAgent>(out m_navMeshAgent))
        {
            Debug.LogWarning("A NPC doesn't have navmesh agent");
        }
        
    }

    public void GoToNextPoint()
    {
        if (m_iPointIndex >= m_crossingPoints.Count - 1)
        {
            Debug.LogWarning("A character tried to go to an out of range position");
            return;
        }

        ++m_iPointIndex;
        NavMesh.SamplePosition(m_crossingPoints[m_iPointIndex].position, out NavMeshHit hit, 5, NavMesh.AllAreas);
        m_navMeshAgent.SetDestination(hit.position);
    }

    public void GoToPreviousPoint()
    {
        if (m_iPointIndex <= 0)
        {
            Debug.LogWarning("A character tried to go to an out of range position");
            return;
        }

        --m_iPointIndex;
        NavMesh.SamplePosition(m_crossingPoints[m_iPointIndex].position, out NavMeshHit hit, 5, NavMesh.AllAreas);
        m_navMeshAgent.SetDestination(hit.position);
    }

    public void GoToSpecificPoint(int destination, bool followingWolePath = true)
    {
        if (destination >= m_crossingPoints.Count)
        {
            Debug.LogWarning("A character tried to go to an out of range position");
            return;
        }

        if (followingWolePath)
        {
            int gap = destination - m_iPointIndex;
            Action incrementFunc = gap < 0 ? GoToPreviousPoint : GoToNextPoint;

            StartCoroutine(CoroutineGoToPointLoop(incrementFunc, Math.Abs(gap) - 1));
        }
        else
        {
            m_iPointIndex = destination;
            NavMesh.SamplePosition(m_crossingPoints[m_iPointIndex].position, out NavMeshHit hit, 5, NavMesh.AllAreas);
            m_navMeshAgent.SetDestination(hit.position);
        }
    }

    private IEnumerator CoroutineGoToPointLoop(Action incrementFunc, int loopAmount)
    {
        incrementFunc();
        Vector3 pathGap = transform.position - m_navMeshAgent.destination;
        pathGap.y = 0;
        while (pathGap.magnitude >= .1)
        {
            pathGap = transform.position - m_navMeshAgent.destination;
            pathGap.y = 0;
            yield return null;
        }
        
        if (loopAmount > 0)
        {
            StartCoroutine(CoroutineGoToPointLoop(incrementFunc, Math.Abs(loopAmount - 1)));
        }
    }

    public void FollowAlongPoints()
    {
        GoToSpecificPoint(m_crossingPoints.Count - 1);
    }

    private void Update()
    {
        //if(Input.GetMouseButtonDown(0))
        //{
        //    GoToPreviousPoint();
        //}
        //if (Input.GetMouseButtonDown(1))
        //{
        //    GoToNextPoint();
        //}
        //if (Input.GetMouseButtonDown(2))
        //{
        //    GoToSpecificPoint(0);
        //}
    }
}
