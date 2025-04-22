using UnityEngine;
using UnityEngine.UI;

public class SoundSettingsManager : MonoBehaviour
{
    private SaveSystemManager saveSystemManager;

    //UI
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    //[SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;


    private void Awake()
    {
        saveSystemManager = SaveSystemManager.Instance;
    }
    private void Start()
    {
        var router = GetComponent<OptionRouter>();

        router.Register("volume_music", val => SetMusicVolume((float)val));
        router.Register("volume_sfx", val => SetSFXVolume((float)val));

        //UI
        musicSlider.onValueChanged.AddListener(val => OptionChangeNotifier.Notify("volume_music", val));
        sfxSlider.onValueChanged.AddListener(val => OptionChangeNotifier.Notify("volume_sfx", val));

        LoadUI();
    }

    private void SetMusicVolume(float vol)
    {
        Debug.Log($"Musique volume: {vol}");
        saveSystemManager.GetGameData().settings.volumeMusic = vol;
        musicSource.volume = vol;
    }

    private void SetSFXVolume(float vol)
    {
        Debug.Log($"SFX volume: {vol}");
        saveSystemManager.GetGameData().settings.volumeSfx = vol;
       /// sfxSource.volume = vol;
    }

    private void LoadUI()
    {
        var data = saveSystemManager.GetGameData();
        musicSlider.value = data.settings.volumeMusic;
        sfxSlider.value = data.settings.volumeSfx;

        // Envoie les valeurs pour synchroniser
        OptionChangeNotifier.Notify("volume_music", data.settings.volumeMusic);
        OptionChangeNotifier.Notify("volume_sfx", data.settings.volumeSfx);

    }
}
