using UnityEngine;

public class MenuAudioManager : MonoBehaviour
{
    public AudioSource[] Audios;

    private void Start()
    {
        Audios = GetComponents<AudioSource>();
    }

    public void ButtonHover()
    {
        Audios[1].Play();
    }

    public void ButtonClick()
    {
        Audios[2].Play();
    }
}
