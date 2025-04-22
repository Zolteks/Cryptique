using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    GameProgressionManager gameProgressionManager;

    void Awake()
    {
        gameProgressionManager = GameProgressionManager.Instance;
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        AudioClip backgroundMusic = gameProgressionManager.GetCurrentRegion().GetBackgroundMusic();
        if (backgroundMusic != null)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = backgroundMusic;
            audioSource.Play();
        }
    }
}

