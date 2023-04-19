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
    [SerializeField]
    private Animator ControlAnim;


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

    public void AllShow(bool a)
    {
        MainAnim.SetBool("Show", a);
        MissionAnim.SetBool("Show", a);
        ControlAnim.SetBool("Show", a);
    }

    public void MainShow(bool a)
    {
        MainAnim.SetBool("Show", a);
    }

    public void MissionSelectShow(bool a)
    {
        MissionAnim.SetBool("Show", a);
    }

    public void ControlsShow(bool a)
    {
        ControlAnim.SetBool("Show", a);
    }



    public void MissionSelect()
    {
        MainShow(false);
        MissionSelectShow(true);
    }

    public void ShowMain()
    {
        AllShow(false);
        MainShow(true);
    }

    public void ShowControls()
    {
        MainShow(false);
        ControlsShow(true);
    }

    //public void Test()
    //{
    //    MissionCompletionTracker.Instance.LoadPlayingMission("1-1");
    //    MissionCompletionTracker.Instance.MissionCompletion(true);
    //}

    //public void ResetPrefs()
    //{
    //    if( PlayerPrefs.HasKey("PlayerLoadout"))
    //    {
    //        PlayerPrefs.DeleteAll();
    //    }
    //}
}
