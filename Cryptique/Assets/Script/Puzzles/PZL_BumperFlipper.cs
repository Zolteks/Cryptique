using System.Collections.Generic;
using UnityEngine;

public class Bumper : MonoBehaviour
{
    public float fInitBumperForces = 500f;

    private float fBumperStrenght;

    private List<Rigidbody> ballsInContact = new List<Rigidbody>();


    public float GetBumperForce()
    {
        return fInitBumperForces;
    }

    public void SetBumperForce(float bumpForce)
    {
        fBumperStrenght = bumpForce;
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
            Vector3 launchDirection = transform.forward;

            rb.velocity = Vector3.zero;

            rb.AddForce(launchDirection * fBumperStrenght, ForceMode.Impulse);
        }
    }

    public void Returnball()
    {
        foreach (Rigidbody rb in ballsInContact)
        {
            Vector3 launchDirection = transform.forward;

            rb.velocity = -rb.velocity;

            rb.AddForce(launchDirection * fInitBumperForces, ForceMode.Impulse);
        }
    }
}
