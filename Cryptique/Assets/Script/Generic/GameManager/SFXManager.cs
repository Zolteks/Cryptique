using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Audio;

public class SFXManager : Singleton<SFXManager>
{
    [SerializeField] private AudioSource templateSFXSource;
    private float sfxVolume = 1f;
    private List<AudioSource> activeSFXSources = new();

    public AudioSource PlaySFX(AudioClip clip, Vector3 position, AudioMixerGroup audioMixer, bool loop = false)
    {
        if (clip == null)  return null;

        GameObject go = new GameObject("SFX_" + clip.name);
        Debug.Log("SFX_" + clip.name);
        go.transform.position = position;

        AudioSource source = go.AddComponent<AudioSource>();
        CopyAudioSettings(templateSFXSource, source);
        source.clip = clip;
        source.volume = sfxVolume;
        source.outputAudioMixerGroup = audioMixer;
        source.loop = loop;
        source.Play();

        activeSFXSources.Add(source);
        if(!loop)
            Destroy(go, clip.length + 0.1f); // Auto-destruction
        return source;
    }

    public void SetSFXVolume(float vol)
    {
        sfxVolume = vol;
        foreach (var src in activeSFXSources)
        {
            if (src != null)
                src.volume = sfxVolume;
        }
    }

    private void CopyAudioSettings(AudioSource from, AudioSource to)
    {
        to.outputAudioMixerGroup = from.outputAudioMixerGroup;
        to.spatialBlend = from.spatialBlend;
        to.rolloffMode = from.rolloffMode;
        to.minDistance = from.minDistance;
        to.maxDistance = from.maxDistance;
        to.playOnAwake = false;
        to.loop = false;
    }
    public float GetSFXVolume() => sfxVolume;

    public void FadeOutAndDestroy(AudioSource source, float duration = 1f)
    {
        if (source != null)
            StartCoroutine(FadeOutCoroutine(source, duration));
    }

    public void FadeOutAllActive(float duration = 1f)
    {
        foreach (var src in activeSFXSources.ToArray()) // copie pour éviter modif de la liste pendant itération
        {
            if (src != null)
                StartCoroutine(FadeOutCoroutine(src, duration));
        }
    }

    private IEnumerator FadeOutCoroutine(AudioSource source, float duration)
    {
        float startVolume = source.volume;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            source.volume = Mathf.Lerp(startVolume, 0, time / duration);
            yield return null;
        }

        source.volume = 0f;
        Destroy(source.gameObject);
        activeSFXSources.Remove(source);
    }
}

