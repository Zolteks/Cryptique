using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Lair : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip lairClip;
    [SerializeField]
    private BoxCollider triggerCollider;
    public void ChangeMusicCave()
    {
        audioSource.clip = lairClip;
        audioSource.Play();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeMusicCave();
            triggerCollider.enabled = false;
        }
    }
}
