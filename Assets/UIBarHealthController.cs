using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBarHealthController : UIBarDisplayController
{
    [SerializeField]
    protected UnityEngine.UI.Image CoatingBar;



    public virtual void UpdateBar(string Text, float Fill, float CoatingFill)
    {
        Display.text = DisplayPrefix + Text;
        Bar.fillAmount = Fill;
        CoatingBar.fillAmount = CoatingFill;

        if (Fill < LowWarningThreshhold && !WarningFlashing)
            Flash(true);
        else if (Fill > LowWarningThreshhold && WarningFlashing)
            Flash(false);
    }
}
