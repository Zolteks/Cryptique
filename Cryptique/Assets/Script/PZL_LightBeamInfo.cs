using System.Collections.Generic;
using UnityEngine;

public class PZL_LightBeamInfo : MonoBehaviour
{
    public List<PZL_IceMirror> mirrorChain = new List<PZL_IceMirror>();
    public PZL_IceMirror sourceMirror;
    public bool isOriginalBeam = false;
    public bool isActive = true;

    public bool HasMirrorInChain(PZL_IceMirror mirror)
    {
        return mirrorChain.Contains(mirror);
    }

    void OnDisable()
    {
        isActive = false;
    }

    void OnEnable()
    {
        isActive = true;
    }
}