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
    GameObject Menu;
    [SerializeField]
    GameObject PlayCanvas;

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
        ShowMenu(!Menu.active);
    }

    public void ShowMenu(bool Show)
    {
        if (Show)
        {
            Menu.SetActive(true);
            PlayCanvas.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Menu.SetActive(false);
            PlayCanvas.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
