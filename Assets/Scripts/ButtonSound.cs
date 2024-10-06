using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource audio;

    public void PlayButton()
    {
        audio.Play();
    }
}
