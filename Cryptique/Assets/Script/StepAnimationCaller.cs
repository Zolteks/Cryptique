using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepAnimationCaller : MonoBehaviour
{
    [SerializeField] ParticleSystem left;
    [SerializeField] ParticleSystem right;

    public void StepLeft()
    {
        left.Play();
    }

    public void StepRight()
    {
        right.Play();
    }
}
