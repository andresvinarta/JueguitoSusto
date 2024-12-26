using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private bool IsActive = false;

    public GameObject OptionsMenu, ExitWarning, MainPauseMenu;

    public void OpenPauseMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        gameObject.SetActive(true);
        OpenMainPauseMenu();
        IsActive = true;
    }

    public void ClosePauseMenu()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        MainPauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        ExitWarning.SetActive(false);
        gameObject.SetActive(false);
        IsActive = false;
        Time.timeScale = 1f;
    }

    public bool IsPauseMenuActive()
    {
        return IsActive;
    }

    public void OpenMainPauseMenu()
    {
        MainPauseMenu.SetActive(true);
        OptionsMenu.SetActive(false);
        ExitWarning.SetActive(false);
    }

    public void OpenOptions()
    {
        MainPauseMenu.SetActive(false);
        OptionsMenu.SetActive(true);
        ExitWarning.SetActive(false);
    }

    public void OpenExitWarning()
    {
        MainPauseMenu.SetActive(false);
        OptionsMenu.SetActive(false);
        ExitWarning.SetActive(true);
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
