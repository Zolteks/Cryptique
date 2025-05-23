using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PZL_Hold : MonoBehaviour
{
    public GameObject gPosRespawnBall;
    public GameObject gBall;

    private void OnTriggerEnter(Collider other)
    {
        gBall.transform.position = gPosRespawnBall.transform.position;
        Rigidbody rb = gBall.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.useGravity = false;
    }
}
