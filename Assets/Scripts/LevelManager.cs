using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class LevelManager : MonoBehaviour
{
    public static event Action<float> FadeIn;
    public static event Action<float> FadeOut;
    public static LevelManager Instance;

    public float fadeDuration = 1.0f;

    // VideoPlayer and clips
    public VideoPlayer skyboxVideoPlayer;  // The VideoPlayer for the skybox
    public VideoClip oneShotVideo;         // The first video to play (one-shot)
    public VideoClip fadeInVideo;          // The fade-in video
    public VideoClip loopedVideo;          // The looped video to play until button press
    public VideoClip fadeOutVideo;         // The fade-out video
    public VideoClip videoAfterFadeOut1;   // Video to play after fade-out
    public VideoClip videoAfterFadeOut2;   // Another video after fade-out

    private bool _isTransitioning = false;  // To check if the transition (fade-in/out) is happening

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Start with the one-shot video
        PlayVideo(oneShotVideo, false);
    }

    // Function to play a video
    public void PlayVideo(VideoClip videoClip, bool loop = false)
    {
        if (skyboxVideoPlayer != null && videoClip != null)
        {
            skyboxVideoPlayer.clip = videoClip;
            skyboxVideoPlayer.isLooping = loop;
            skyboxVideoPlayer.Play();
        }
    }

    // Function to transition to fade-in and looped video
    public void PlayFadeInAndLoop()
    {
        if (!_isTransitioning)
        {
            StartCoroutine(FadeInAndLoop());
        }
    }

    // Coroutine to fade in and start the looped video
    private IEnumerator FadeInAndLoop()
    {
        _isTransitioning = true;

        // Play the fade-in video and start the fade-in effect
        PlayVideo(fadeInVideo, false);
        FadeIn?.Invoke(fadeDuration);
        yield return new WaitForSeconds(fadeDuration + (float)fadeInVideo.length);

        // Play the looped video once fade-in is complete
        PlayVideo(loopedVideo, true);

        _isTransitioning = false;
    }

    // ** This function can be called from OnClick in a Unity button **
    public void StopLoopAndPlayFadeOut()
    {
        if (!_isTransitioning)
        {
            StartCoroutine(FadeOutAndPlayNextVideos());
        }
    }

    // Coroutine to fade out, stop the looped video, and play the next videos
    private IEnumerator FadeOutAndPlayNextVideos()
    {
        _isTransitioning = true;

        // Play the fade-out video and trigger fade-out effect
        PlayVideo(fadeOutVideo, false);
        FadeOut?.Invoke(fadeDuration);
        yield return new WaitForSeconds(fadeDuration + (float)fadeOutVideo.length);

        // After fade-out finishes, play the first additional video
        PlayVideo(videoAfterFadeOut1, false);
        yield return new WaitForSeconds((float)videoAfterFadeOut1.length);

        // Play the second additional video
        PlayVideo(videoAfterFadeOut2, false);
        yield return new WaitForSeconds((float)videoAfterFadeOut2.length);

        // Once these videos finish, return to the fade-in and loop sequence
        PlayFadeInAndLoop();

        _isTransitioning = false;
    }

    // Optional: Clean up event listeners if needed
    private void OnDestroy()
    {
        FadeIn = null;
        FadeOut = null;
    }
}
