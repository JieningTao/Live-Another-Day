using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEXGearDisplay : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Image Background;
    [SerializeField]
    UnityEngine.UI.Image BackgroundFill;
    [SerializeField]
    Color StandByColor;
    [SerializeField]
    Color UnavaliableColor;
    [SerializeField]
    Color SelectedColor;
    [SerializeField]
    [Range(0,1)]
    float FillPercentage;

    [Space(20)]

    [SerializeField]
    int SlotNumber;
    [SerializeField]
    UnityEngine.UI.Image Icon;
    [SerializeField]
    UnityEngine.UI.Text Number;
    [SerializeField]
    UnityEngine.UI.Text Name;

    [SerializeField]
    Animator MyAnimator;

    protected BaseEXGear MyEXGear;

    private void Start()
    {
        //Number.text = SlotNumber + "";
    }

    private void EXGearInitialize(BaseEXGear Gear)
    {
        MyEXGear = Gear;
        if (Gear == null)
        {
            //Debug.Log(SlotNumber);
            SetBGColor(UnavaliableColor);
            Icon.gameObject.SetActive(false);
            Name.text = "";
            MyAnimator.SetBool("Avaliable", false);
            return;
        }
        else
        {
            Gear.GetInitializeData(out Sprite a, out string b);
            Icon.sprite = a;
            Name.text = b;

            if (Gear.EXGIsPassive)
            {
                SetBGColor(UnavaliableColor);
                MyAnimator.SetBool("Avaliable", false);
            }
            else
            {
                SetBGColor(StandByColor);
                MyAnimator.SetBool("Avaliable", true);
            }
        }
    }

    private void SetBGColor(Color a)
    {
        Color BGC = a;
        BGC.a *= (1 - FillPercentage);
        Background.color = BGC;

        Color BGFC = a;
        BGC.a *= FillPercentage;
        BackgroundFill.color = BGFC;
    }

    private void Update()
    {
        if (MyEXGear != null)
        {
            BackgroundFill.fillAmount = MyEXGear.GetReadyPercentage();
        }
    }

    private void RecieveNews(int a, string b, BaseEXGear c)
    {
        //return;
        if (a == SlotNumber)
        {
            if (b == "New")
                EXGearInitialize(c);
        }

        if (b == "Select")
        {
            if (a == SlotNumber)
                MyAnimator.SetBool("Selected", true);
            else
                MyAnimator.SetBool("Selected", false);
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
