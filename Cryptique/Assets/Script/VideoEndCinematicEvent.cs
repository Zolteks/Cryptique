using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

[RequireComponent(typeof(VideoPlayer))]
public class VideoEndCinematicEvent : MonoBehaviour
{
    public UnityEvent onVideoEnd;

    private VideoPlayer videoPlayer;

    private void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.loopPointReached += HandleVideoEnd;
    }

    private void HandleVideoEnd(VideoPlayer vp)
    {
        Debug.Log("Video finished, triggering event!");
        onVideoEnd?.Invoke();
    }

    private void OnDestroy()
    {
        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached -= HandleVideoEnd;
        }
    }
}
