using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_Hit : MonoBehaviour
{

    public int iHealthPoint = 0;

    private void Start()
    {
        if (iHealthPoint < 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TakeDamage();
    }

    void TakeDamage()
    {
        iHealthPoint -= 1;

        if (iHealthPoint == 0)
        {
            Destroy(gameObject);
        }
    }
}
