using UnityEngine;
using UnityEngine.Video;

public class VideoLoopManager : MonoBehaviour
{
    // References to the video players
    public VideoPlayer videoEnterOnePlay;   // First video: entering the elevator
    public VideoPlayer videoFadeIntoElevator;  // Second video: fade into elevator
    public VideoPlayer videoStandLooped;    // Third video: standing in the elevator (looped)
    public VideoPlayer videoFadeOut;        // Final video: fade out (end)

    // New videos for the sequence after the fade-out
    public VideoPlayer videoExtraOne;    // First video after fade-out
    public VideoPlayer videoExtraTwo;    // Second video after fade-out

    // GameObjects for controlling visibility
    public GameObject videoEnterOnePlayObject;  // GameObject for Video-Enter-OnePlay
    public GameObject videoFadeIntoElevatorObject;  // GameObject for Fade into elevator video
    public GameObject videoStandLoopedObject;   // GameObject for standing looped video
    public GameObject videoFadeOutObject;       // GameObject for Fade-out video
    public GameObject videoExtraOneObject;      // GameObject for the first extra video
    public GameObject videoExtraTwoObject;      // GameObject for the second extra video

    private bool videoEndCalled = false;  // To track if the ending sequence is called
    private bool inExtraSequence = false; // To track if we are in the extra video sequence

    void Start()
    {
        // Subscribe to the video finished events
        videoEnterOnePlay.loopPointReached += OnEnterOnePlayFinished;
        videoFadeIntoElevator.loopPointReached += OnFadeIntoElevatorFinished;
        videoStandLooped.loopPointReached += OnLoopVideoReached;  // Will keep playing because of looping
        videoFadeOut.loopPointReached += OnFadeOutFinished;
        videoExtraOne.loopPointReached += OnExtraOneFinished;
        videoExtraTwo.loopPointReached += OnExtraTwoFinished;
    }

    // First video finished (Entering the elevator)
    void OnEnterOnePlayFinished(VideoPlayer vp)
    {
        // Deactivate Video-Enter-OnePlay GameObject
        videoEnterOnePlayObject.SetActive(false);

        // Activate Fade-into-elevator video
        videoFadeIntoElevatorObject.SetActive(true);
        videoFadeIntoElevator.Play();
    }

    // Second video finished (Fade into elevator)
    void OnFadeIntoElevatorFinished(VideoPlayer vp)
    {
        // Deactivate Fade-into-elevator GameObject
        videoFadeIntoElevatorObject.SetActive(false);

        // Activate looped video (standing in the elevator)
        videoStandLoopedObject.SetActive(true);
        videoStandLooped.isLooping = true;
        videoStandLooped.Play();
    }

    // Loop video reached (this happens repeatedly due to looping)
    void OnLoopVideoReached(VideoPlayer vp)
    {
        // You can check for any condition to end the loop (such as a user action)
        if (videoEndCalled)
        {
            EndVideoSequence();
        }
    }

    // Method to call the fade-out (e.g., when the elevator journey ends)
    public void EndVideoSequence()
    {
        // Stop the looped video
        videoStandLooped.Stop();
        videoStandLoopedObject.SetActive(false);

        // Activate the fade-out video
        videoFadeOutObject.SetActive(true);
        videoFadeOut.Play();
    }

    // Final video (Fade-out) finished
    void OnFadeOutFinished(VideoPlayer vp)
    {
        // Deactivate fade-out GameObject
        videoFadeOutObject.SetActive(false);

        // Start the extra sequence
        StartExtraSequence();
    }

    // Start playing extra sequence (two videos in a row)
    void StartExtraSequence()
    {
        inExtraSequence = true;

        // Play the first extra video
        videoExtraOneObject.SetActive(true);
        videoExtraOne.Play();
    }

    // First extra video finished
    void OnExtraOneFinished(VideoPlayer vp)
    {
        // Deactivate first extra video
        videoExtraOneObject.SetActive(false);

        // Play the second extra video
        videoExtraTwoObject.SetActive(true);
        videoExtraTwo.Play();
    }

    // Second extra video finished
    void OnExtraTwoFinished(VideoPlayer vp)
    {
        // Deactivate second extra video
        videoExtraTwoObject.SetActive(false);

        // Return to the looped video sequence
        RestartLoopedSequence();
    }

    // Return to the looped fade-in/fade-out sequence
    void RestartLoopedSequence()
    {
        inExtraSequence = false;

        // Activate Fade-into-elevator video (like resetting the sequence)
        videoFadeIntoElevatorObject.SetActive(true);
        videoFadeIntoElevator.Play();
    }

    public void StartSequence()
    {
        // Start the first video sequence
        videoEnterOnePlayObject.SetActive(true);
        videoEnterOnePlay.Play();
    }

    // Optional method to clean up the event subscriptions when the object is destroyed
    void OnDestroy()
    {
        videoEnterOnePlay.loopPointReached -= OnEnterOnePlayFinished;
        videoFadeIntoElevator.loopPointReached -= OnFadeIntoElevatorFinished;
        videoStandLooped.loopPointReached -= OnLoopVideoReached;
        videoFadeOut.loopPointReached -= OnFadeOutFinished;
        videoExtraOne.loopPointReached -= OnExtraOneFinished;
        videoExtraTwo.loopPointReached -= OnExtraTwoFinished;
    }
}
