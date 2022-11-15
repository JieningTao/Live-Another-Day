using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDBottomBarCenter : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Image MainBar;
    [SerializeField]
    UnityEngine.UI.Image SubBar;
    [SerializeField]
    UnityEngine.UI.Text MainText;
    [SerializeField]
    UnityEngine.UI.Text SubText;
    [SerializeField]
    UnityEngine.UI.Text Title;

    BaseEXGear[] AllEXGs =  new BaseEXGear[8];

    BaseEXGear CurrentSelectedEXG;
    Sprite EXGSprite;

    private void Update()
    {
        if (CurrentSelectedEXG)
        {
            MainBar.fillAmount = CurrentSelectedEXG.GetReadyPercentage();
            SubBar.fillAmount = CurrentSelectedEXG.GetSubReadyPercentage();

            MainText.text = CurrentSelectedEXG.GetBBMainText();
            SubText.text = CurrentSelectedEXG.GetBBSubText();
        }
    }

    private void RecieveNews(int a, string b, BaseEXGear c)
    {

        if (b == "New")
        {
            AllEXGs[a-1] = c;
        }

        else if (b == "Select")
        {
            CurrentSelectedEXG = AllEXGs[a-1];
            Title.text = CurrentSelectedEXG.GetName();
        }
    }


    private void OnEnable()
    {
        BaseMechFCS.EXGearChanges += RecieveNews;
    }

    private void OnDisable()
    {
        BaseMechFCS.EXGearChanges -= RecieveNews;
    }
}
