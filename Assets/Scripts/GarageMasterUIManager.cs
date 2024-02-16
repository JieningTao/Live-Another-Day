using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GarageMasterUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ExitConfirm;

    private void Start()
    {
        UnlockTagTracker.Instance.GarageEntered();    
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ToggleExitConfirm(bool a)
    {
        ExitConfirm.SetActive(a);
    }
}
