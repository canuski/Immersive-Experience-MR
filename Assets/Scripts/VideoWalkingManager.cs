using UnityEngine;
using UnityEngine.Video;

public class VideoWalkManager : MonoBehaviour
{
    public VideoPlayer additionalVideoPlayer; // The video player for the additional video
    public GameObject additionalVideoObject;   // The GameObject for the additional video

    private VideoLoopManager videoLoopManager; // Reference to the VideoLoopManager

    void Start()
    {
        // Get the VideoLoopManager component attached to another GameObject in the scene
        videoLoopManager = FindObjectOfType<VideoLoopManager>();

        // Ensure the additional video is inactive initially
        additionalVideoObject.SetActive(false);

        // Subscribe to the finished event of the additional video
        additionalVideoPlayer.loopPointReached += OnAdditionalVideoFinished;
    }

    // Method to play the additional video when the button is pressed
    public void PlayAdditionalVideo()
    {
        // Stop the current video sequence if it's running
        if (videoLoopManager != null)
        {
            videoLoopManager.EndVideoSequence(); // Call to end the current sequence
        }

        // Activate the additional video GameObject and play the video
        additionalVideoObject.SetActive(true);
        additionalVideoPlayer.Play();
    }

    // Called when the additional video finishes
    void OnAdditionalVideoFinished(VideoPlayer vp)
    {
        // Deactivate the additional video GameObject
        additionalVideoObject.SetActive(false);

        // Restart the video sequence in VideoLoopManager
        if (videoLoopManager != null)
        {
            videoLoopManager.StartSequence(); // Ensure to implement StartSequence in VideoLoopManager
        }
    }

    // Cleanup on destroy
    void OnDestroy()
    {
        additionalVideoPlayer.loopPointReached -= OnAdditionalVideoFinished;
    }
}
