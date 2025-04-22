using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    SaveSystemManager saveSystemManager;
    GameProgressionManager gameProgression;

    void Awake()
    {
        saveSystemManager = SaveSystemManager.Instance;
        gameProgression = GameProgressionManager.Instance;
    }

    void Start()
    {
        PlayBackgroundMusic();
    }

    public void PlayBackgroundMusic()
    {
        AudioClip backgroundMusic = gameProgression.GetRegionByName(saveSystemManager.GetGameData().progression.currentRegion).GetBackgroundMusic();
        if (backgroundMusic != null)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = backgroundMusic;
            audioSource.Play();
        }
    }
}

