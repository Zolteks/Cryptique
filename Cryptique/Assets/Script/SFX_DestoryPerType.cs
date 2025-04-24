
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SFX_DestoryPerType : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> audioClips;
    public void DestroySFX()
    {

        foreach (var clip in audioClips)
        {
            if (clip != null)
            {
                Destroy(clip, 1f);
            }
        }
    }
}

