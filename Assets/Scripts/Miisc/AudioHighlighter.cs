using UnityEngine;

public class AudioHighlighter : MonoBehaviour
{
    private AudioSource SoundToPlayOnLoop;

    public int AudioSourceIndex = 0;

    public bool PlaySoundOnPlayerTrigger = false;

    private void Start()
    {
        SoundToPlayOnLoop = GetComponents<AudioSource>()[AudioSourceIndex];
        if (SoundToPlayOnLoop != null && !SoundToPlayOnLoop.loop) SoundToPlayOnLoop.loop = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlaySoundOnPlayerTrigger && other.gameObject.tag == "Player")
        {
            PlaySound();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (PlaySoundOnPlayerTrigger && other.gameObject.tag == "Player")
        {
            StopSound();
        }
    }

    public void PlaySound()
    {
        SoundToPlayOnLoop.enabled = true;
    }

    public void StopSound()
    {
        SoundToPlayOnLoop.enabled = false;
    }
}
