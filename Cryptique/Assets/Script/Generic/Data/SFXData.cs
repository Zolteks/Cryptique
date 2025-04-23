using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SFXData", menuName = "Cryptique/SFX")]

public class SFXData : ScriptableObject
{
    [System.Serializable]
    public class SFX
    {
        public string name;
        public AudioClip clip;
    }

    [SerializeField]
    public List<SFX> sfxClips;

    public List<SFX> SFXClips()
    {
        return sfxClips;
    }

    public SFX GetSFXByName(string name)
    {
        foreach (SFX sfx in sfxClips)
        {
            if (sfx.name == name)
            {
                return sfx;
            }
        }
        Debug.LogWarning($"SFX with name {name} not found.");
        return null;
    }

    public string[] GetSFXNames()
    {
        List<string> names = new List<string>();
        foreach (var sfx in sfxClips)
        {
            names.Add(sfx.name);
        }
        return names.ToArray();
    }
}

