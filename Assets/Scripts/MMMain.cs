using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MMMain : MonoBehaviour
{
    [SerializeField]
    private Animator MainAnim;
    [SerializeField]
    private Animator MissionAnim;


    private void Start()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Garage()
    {
        SceneManager.LoadScene("Garage");
    }

    public void MissionSelect()
    {
        MainAnim.SetBool("Show", false);
        MissionAnim.SetBool("Show", true);
    }

    public void ShowMain()
    {
        MainAnim.SetBool("Show", true);
        MissionAnim.SetBool("Show",false);
    }

    //public void ResetPrefs()
    //{
    //    if( PlayerPrefs.HasKey("PlayerLoadout"))
    //    {
    //        PlayerPrefs.DeleteAll();
    //    }
    //}
}
