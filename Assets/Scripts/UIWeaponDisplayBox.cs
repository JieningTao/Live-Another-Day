using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWeaponDisplayBox : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text Name;
    [SerializeField]
    UnityEngine.UI.Text Ammo;
    [SerializeField]
    UnityEngine.UI.Image Fillbar;
    [SerializeField]
    GameObject ContentParent;
    [SerializeField]
    UnityEngine.UI.Image BG;
    [SerializeField]
    Color BGAvaliableColor;
    [SerializeField]
    Color BGUnavaliableColor;
    [SerializeField]
    GameObject Warning;
    [SerializeField]
    UnityEngine.UI.Image WarningAmmo;
    [SerializeField]
    UnityEngine.UI.Image WarningPower;


    bool RightWeapon;
    bool MainFire;




    public void StartInitialize(string WeaponName,Color BarColor,bool Right,bool Main)
    {
        Name.text = WeaponName;
        Fillbar.color = BarColor;
        RightWeapon = Right;
        MainFire = Main;
    }

    public void RegularUpdate(string _Ammo,float FillPercentage)
    {
        Ammo.text = _Ammo;
        Fillbar.fillAmount = FillPercentage;
    }

    public void SetAvaliable(bool Avaliable)
    {
        ContentParent.SetActive(Avaliable);

        if (Avaliable)
            BG.color = BGAvaliableColor;
        else
            BG.color = BGUnavaliableColor;
    }

    public void AmmoWarning(bool active)
    {
        if (active)
        {
            WarningAmmo.gameObject.SetActive(true);
            Warning.SetActive(true);
        }
        else
        {
            WarningAmmo.gameObject.SetActive(false);
            if(!WarningPower.gameObject.active)
                Warning.SetActive(false);
        }
    }

    public void EnergyWarning(bool active)
    {
        if (active)
        {
            WarningPower.gameObject.SetActive(true);
            Warning.SetActive(true);
        }
        else
        {
            WarningPower.gameObject.SetActive(false);
            if (!WarningAmmo.gameObject.active)
                Warning.SetActive(false);
        }
    }

    public void WeaponWarnings(bool Right, bool Main, bool Ammo, bool Active)
    {
        if (Right == RightWeapon && Main == MainFire)
        {
            if (Ammo)
                AmmoWarning(Active);
            else
                EnergyWarning(Active);
        }
    }

    private void OnEnable()
    {
        BaseMechFCS.WeaponWarnings += WeaponWarnings;
    }

    private void OnDisable()
    {
        BaseMechFCS.WeaponWarnings -= WeaponWarnings;
    }



}
