using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowTrailHandler : MonoBehaviour
{
    [SerializeField] RenderTexture trailMap;
    [SerializeField] ParticleSystem particle;


    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            ResetTrail();
        }
    }

    void ResetTrail()
    {
        particle.Stop();
        RenderTexture rt = RenderTexture.active;
        RenderTexture.active = trailMap;
        GL.Clear(true, true, Color.black);
        RenderTexture.active = rt;
        trailMap.Release();
        particle.Clear();
        particle.Play();
    }
}
