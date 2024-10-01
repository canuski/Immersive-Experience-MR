using UnityEngine;
using UnityEngine.Video;

public class VideoLoopManager : MonoBehaviour
{
    // References to the video players
    public VideoPlayer videoEnterOnePlay;   // First video: entering the elevator
    public VideoPlayer videoFadeIntoElevator;  // Second video: fade into elevator
    public VideoPlayer videoStandLooped;    // Third video: standing in the elevator (looped)
    public VideoPlayer videoFadeOut;        // Final video: fade out (end)

    // GameObjects for controlling visibility
    public GameObject videoEnterOnePlayObject;  // GameObject for Video-Enter-OnePlay
    public GameObject videoFadeIntoElevatorObject;  // GameObject for Fade into elevator video
    public GameObject videoStandLoopedObject;   // GameObject for standing looped video
    public GameObject videoFadeOutObject;       // GameObject for Fade-out video

    private bool videoEndCalled = false;  // To track if the ending sequence is called

    void Start()
    {
        // Subscribe to the video finished events
        videoEnterOnePlay.loopPointReached += OnEnterOnePlayFinished;
        videoFadeIntoElevator.loopPointReached += OnFadeIntoElevatorFinished;
        videoStandLooped.loopPointReached += OnLoopVideoReached;  // Will keep playing because of looping
        videoFadeOut.loopPointReached += OnFadeOutFinished;
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

    public void StartSequence()
    {
        // Start the first video sequence
        videoEnterOnePlayObject.SetActive(true);
        videoEnterOnePlay.Play();
    }


    // Final video (Fade-out) finished
    void OnFadeOutFinished(VideoPlayer vp)
    {
        // Do any cleanup or further actions needed after the final fade-out
        videoFadeOutObject.SetActive(false);
        Debug.Log("Elevator sequence completed.");
    }

    // Optional method to clean up the event subscriptions when the object is destroyed
    void OnDestroy()
    {
        videoEnterOnePlay.loopPointReached -= OnEnterOnePlayFinished;
        videoFadeIntoElevator.loopPointReached -= OnFadeIntoElevatorFinished;
        videoStandLooped.loopPointReached -= OnLoopVideoReached;
        videoFadeOut.loopPointReached -= OnFadeOutFinished;
    }
}
