using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBarDisplayController : MonoBehaviour
{
    [SerializeField]
    protected UnityEngine.UI.Image Bar;
    [SerializeField]
    protected UnityEngine.UI.Text Display;
    [SerializeField]
    protected GameObject Warning;

    [SerializeField]
    protected string DisplayPrefix;
    [SerializeField]
    protected float LowWarningThreshhold = 0.2f;
    [SerializeField]
    protected float FlashDuration = 0.3f;
    protected float CurrentFlashCooldown;
    protected bool WarningFlashing = false;


    public virtual void UIInitialize()
    {
        Warning.SetActive(false);
    }


    protected void Update()
    {
        UpdateFlash();
    }



    public virtual void UpdateBar(string Text,float Fill)
    {
        Display.text = DisplayPrefix + Text;
        Bar.fillAmount = Fill;

        if (Fill < LowWarningThreshhold && !WarningFlashing)
            Flash(true);
        else if (Fill > LowWarningThreshhold && WarningFlashing)
            Flash(false);
    }

    protected void Flash(bool Start)
    {
        if (Start)
        {
            WarningFlashing = true;
            Warning.SetActive(true);
            CurrentFlashCooldown = FlashDuration;
        }
        else
        {
            WarningFlashing = false;
            Warning.SetActive(false);
            CurrentFlashCooldown = FlashDuration;
        }
    }

    protected void UpdateFlash()
    {
        if (WarningFlashing)
        {
            CurrentFlashCooldown -= Time.deltaTime;
            if (CurrentFlashCooldown <= 0)
            {
                    Warning.SetActive(!Warning.active);
                CurrentFlashCooldown = FlashDuration;
            }
        }
    }
}
