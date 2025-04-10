using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PZL_LairWendigoTriggerTimer : MonoBehaviour
{
    [SerializeField] private GameObject m_WallWendigo;
    [SerializeField] private float fTimerDelay;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(TimerRoutine());
    }

    IEnumerator TimerRoutine()
    {
        yield return new WaitForSeconds(fTimerDelay);
        
        Destroy(m_WallWendigo);
    }
}
