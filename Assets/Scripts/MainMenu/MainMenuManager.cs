using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject MainCanvas, OptionsCanvas, CreditsCanvas, FadeCanvas, AudioManager;


    [SerializeField]
    private string FirstSceneName;

    private bool FadingToBlack = false;

    private float TimeToFade, TimePassed = 0, TimeToLoad;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        MainCanvas.SetActive(true);
        OptionsCanvas.SetActive(false);
        CreditsCanvas.SetActive(false);
        TimeToLoad = AudioManager.GetComponents<AudioSource>()[3].clip.length;
        TimeToFade = AudioManager.GetComponents<AudioSource>()[3].clip.length * 0.65f;
    }

    private void Update()
    {
        if (FadingToBlack)
        {
            TimePassed += Time.deltaTime;
            if (FadeCanvas.GetComponent<CanvasGroup>().alpha < 1)
            {
                FadeCanvas.GetComponent<CanvasGroup>().alpha = Mathf.Lerp(0, 1, TimePassed / TimeToFade);
            }
            else if (TimePassed >= TimeToLoad)
            {
                FadingToBlack = false;
                LoadFirstScene();
            }
        }
    }

    public void OpenOptions()
    {
        MainCanvas.SetActive(false);
        OptionsCanvas.SetActive(true);
    }

    public void CloseOptions() 
    {
        OptionsCanvas.SetActive(false);
        MainCanvas.SetActive(true);
    }

    public void OpenCredits()
    {
        MainCanvas.SetActive(false);
        CreditsCanvas.SetActive(true);
    }

    public void CloseCredits() 
    {
        CreditsCanvas.SetActive(false);
        MainCanvas.SetActive(true);
    }

    public void Play()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioManager.GetComponents<AudioSource>()[0].Stop();
        AudioManager.GetComponents<AudioSource>()[3].Play();
        FadingToBlack = true;
    }

    public void Exit()
    {
        Application.Quit();
    }

    private void LoadFirstScene()
    {
        SceneManager.LoadScene(FirstSceneName);
    }

}
