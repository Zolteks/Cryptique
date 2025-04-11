using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    [SerializeField] private Camera m_mainCamera;
    [SerializeField] private Camera m_gameCamera;
    [SerializeField] private GameObject m_UI;

    private AudioListener m_mainAudioListener;
    private AudioListener m_gameAudioListener;

    void Start()
    {
        m_mainAudioListener = m_mainCamera.GetComponent<AudioListener>();
        m_gameAudioListener = m_gameCamera.GetComponent<AudioListener>();

        m_mainCamera.enabled = true;
        m_gameCamera.enabled = false;

        if (m_mainAudioListener != null) m_mainAudioListener.enabled = true;
        if (m_gameAudioListener != null) m_gameAudioListener.enabled = false;

        m_UI.SetActive(false);
    }

    void OnMouseDown()
    {
        m_mainCamera.enabled = false;
        m_gameCamera.enabled = true;

        if (m_mainAudioListener != null) m_mainAudioListener.enabled = false;
        if (m_gameAudioListener != null) m_gameAudioListener.enabled = true;

        m_UI.SetActive(true);
    }

    public void QuitGame()
    {
        m_gameCamera.enabled = false;
        m_mainCamera.enabled = true;

        if (m_gameAudioListener != null) m_gameAudioListener.enabled = false;
        if (m_mainAudioListener != null) m_mainAudioListener.enabled = true;

        m_UI.SetActive(false);
    }
}
