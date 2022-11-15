using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMiniMenu : MonoBehaviour
{
    public static PauseMiniMenu _instance;
    public static PauseMiniMenu Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PauseMiniMenu>();
            }
            return _instance;
        }
    }

    [SerializeField]
    GameObject PauseMenu;
    [SerializeField]
    GameObject PlayCanvas;
    [SerializeField]
    GameObject EndLevelScreen;
    [SerializeField]
    UnityEngine.UI.Text EndText;

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Sound(float Volume)
    {

    }

    public void ToggleMenu()
    {
        if (!EndLevelScreen.active)
        {
            if (!PauseMenu.active)
                ShowPauseUI();
            else
                ShowPlayUI();
        }
    }

    public void ShowPauseUI()
    {
        PauseMenu.SetActive(true);
        PlayCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0;
    }

    public void ShowPlayUI()
    {
        PauseMenu.SetActive(false);
        PlayCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;
    }

    public void ShowLevelEndUI(bool Win)
    {
        if (Win)
        {
            EndText.text = "Mission Complete";
            Time.timeScale = 0;
        }
        else
        {
            EndText.text = "Mission Failed";
            Time.timeScale = 1;
        }


        PauseMenu.SetActive(false);
        PlayCanvas.SetActive(false);
        EndLevelScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        
    }

}
