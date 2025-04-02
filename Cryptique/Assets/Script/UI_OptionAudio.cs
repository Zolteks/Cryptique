using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OptionAudio : MonoBehaviour
{
    [SerializeField] private Slider MusicSlider;
    [SerializeField] private Slider SFXSlider;

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        MusicSlider.value = audioSource.volume;
        SFXSlider.value = audioSource.volume;
    }

    public void SetMusic()
    {
        audioSource.volume = MusicSlider.value;
    }

    public void SetSFX()
    {
        audioSource.volume = SFXSlider.value;
    }
}

