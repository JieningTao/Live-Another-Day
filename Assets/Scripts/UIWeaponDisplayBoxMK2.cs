using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWeaponDisplayBoxMK2 : MonoBehaviour
{
    [SerializeField]
    bool RightWeapon;
    [SerializeField]
    UnityEngine.UI.Image BG;
    [SerializeField]
    GameObject ContentParent;
    [SerializeField]
    GameObject SecondaryContentParent;
    [SerializeField]
    UnityEngine.UI.Image Mask;

    [Space(20)]

    [SerializeField]
    UnityEngine.UI.Text Name;
    [SerializeField]
    UnityEngine.UI.Text AmmoMain;
    [SerializeField]
    UnityEngine.UI.Image FillbarMain;
    [SerializeField]
    UnityEngine.UI.Text AmmoSecondary;
    [SerializeField]
    UnityEngine.UI.Image FillbarSecondary;
    [SerializeField]
    Animator ECWarning;

    [Space(20)]

    [SerializeField]
    Color BGAvaliableColor;
    [SerializeField]
    Color BGUnavaliableColor;
    [SerializeField]
    GameObject Warning;
    [SerializeField]
    GameObject WarningAmmo;
    [SerializeField]
    GameObject WarningPower;


    bool HaveSecondary;
    BaseMainSlotEquipment Equipment;





    private void Update()
    {
        if(Equipment)
        {
            Equipment.GetUpdateData(true, out float BFP, out string TD);
            UpdateMain(TD, BFP);

            if(HaveSecondary)
            {
                Equipment.GetUpdateData(false, out float SBFP, out string STD);
                UpdateSecondary(STD, SBFP);
            }
        }
    }

    private void UpdateMain(string Ammo, float FillPercentage)
    {
        AmmoMain.text = Ammo;
        FillbarMain.fillAmount = FillPercentage;
    }

    private void UpdateSecondary(string Ammo, float FillPercentage)
    {
        AmmoSecondary.text = Ammo;
        FillbarSecondary.fillAmount = FillPercentage;
    }

    private void SetSecondaryAvaliable(bool Avaliable)
    {
        HaveSecondary = Avaliable;
        if (Avaliable)
        {
            Mask.fillAmount = 1f;
            SecondaryContentParent.SetActive(true);
        }
        else
        {
            Mask.fillAmount = 0.68f;
            SecondaryContentParent.SetActive(false);
        }

    }

    private void SetAvaliable(bool Avaliable)
    {
        ContentParent.SetActive(Avaliable);

        if (Avaliable)
        {
            BG.color = BGAvaliableColor;

        }
        else
        {
            BG.color = BGUnavaliableColor;
            SetSecondaryAvaliable(false);
        }
    }

    private void NewWeapon(bool Right, BaseMainSlotEquipment _Equipment)
    {
        if (Right == RightWeapon)
        {
            Equipment = _Equipment;
            if (!_Equipment)
            {
                SetAvaliable(false);
                return;
            }

            Equipment.GetInitializeDate(out string MainName, out Color MainColor, out string SecondaryName, out Color SecondaryColor);

            Name.text = MainName;
            FillbarMain.color = MainColor;

            if (SecondaryName != "\n" && SecondaryName != "")
            {
                SetSecondaryAvaliable(true);
                FillbarSecondary.color = SecondaryColor;
            }
            else
                SetSecondaryAvaliable(false);

            ECWarning.SetTrigger("EquipmentChanged");
        }
    }

    public void AmmoWarning(bool active)
    {
        if (active)
        {
            WarningAmmo.SetActive(true);
            Warning.SetActive(true);
        }
        else
        {
            WarningAmmo.SetActive(false);
            if (!WarningPower.gameObject.active)
                Warning.SetActive(false);
        }
    }

    public void EnergyWarning(bool active)
    {
        if (active)
        {
            WarningPower.SetActive(true);
            Warning.SetActive(true);
        }
        else
        {
            WarningPower.SetActive(false);
            if (!WarningAmmo.gameObject.active)
                Warning.SetActive(false);
        }
    }

    public void WeaponWarnings(bool Right, bool Main, bool Ammo, bool Active)
    {
        if (Right == RightWeapon)
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
        BaseMechFCS.WeaponChanges += NewWeapon;
    }

    private void OnDisable()
    {
        BaseMechFCS.WeaponWarnings -= WeaponWarnings;
        BaseMechFCS.WeaponChanges -= NewWeapon;
    }
}
