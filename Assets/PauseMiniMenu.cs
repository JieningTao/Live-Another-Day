using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMiniMenu : MonoBehaviour
{


    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
