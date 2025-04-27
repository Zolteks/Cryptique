using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioVolumeRiser : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField]
    private float startVolume = 0f;
    [SerializeField]
    private float targetVolume = 1f;
    [SerializeField]
    private float riseDuration = 10f;     

    private AudioSource audioSource;
    private float timer = 0f;
    private bool isRising = true;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = startVolume;
    }

    private void Update()
    {
        if (!isRising) return;

        timer += Time.deltaTime;

        float t = Mathf.Clamp01(timer / riseDuration);
        audioSource.volume = Mathf.Lerp(startVolume, targetVolume, t);

        if (timer >= riseDuration)
        {
            audioSource.volume = targetVolume;
            isRising = false;
        }
    }
}
