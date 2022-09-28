using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBarSpeedController : UIBarDisplayController
{
    [SerializeField]
    protected UnityEngine.UI.Image NormalSpeedBar;

    public void UIInitialize(float NormSpeedLimitRatio)
    {
        base.UIInitialize();

        NormalSpeedBar.fillAmount = NormSpeedLimitRatio * 0.8f;

    }


    public override void UpdateBar(string Text, float Fill)
    {
        Display.text = DisplayPrefix + Text;
        Bar.fillAmount = Fill;

        if (Fill > (1-LowWarningThreshhold) && !WarningFlashing)
            Flash(true);
        else if (Fill < (1-LowWarningThreshhold) && WarningFlashing)
            Flash(false);
    }
}
