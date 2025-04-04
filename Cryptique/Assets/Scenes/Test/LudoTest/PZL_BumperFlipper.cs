using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float bumperForce = 500f;

    private List<Rigidbody> ballsInContact = new List<Rigidbody>();

    public void SetBumperForce(float bumpForce)
    {
        bumperForce = bumpForce;
    }

    void OnTriggerEnter(Collider collision)
    {
        AddBall(collision);
    }

    void OnTriggerStay(Collider collision)
    {
        AddBall(collision);
    }

    void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null && ballsInContact.Contains(rb))
            {
                ballsInContact.Remove(rb);
            }
        }
    }

    private void AddBall(Collider collision)
    {
        if (collision.gameObject.CompareTag("Ball"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            if (rb != null && !ballsInContact.Contains(rb))
            {
                ballsInContact.Add(rb);
            }
        }
    }

    public void LaunchBalls()
    {
        foreach (Rigidbody rb in ballsInContact)
        {
            Vector3 launchDirection = Vector3.forward;

            rb.velocity = Vector3.zero;

            rb.AddForce(launchDirection * bumperForce, ForceMode.Impulse);
        }
    }
}
