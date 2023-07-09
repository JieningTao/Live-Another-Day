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

    [Space(20)]

    [Header("Config 1 for two weapon layout")]
    [SerializeField]
    GameObject Config1Parent;
    [SerializeField]
    UnityEngine.UI.Text AmmoSecondary;
    [SerializeField]
    UnityEngine.UI.Image FillbarSecondary;
    [Header("Config 1 for ModeSwitch Weapon")]
    [SerializeField]
    GameObject Config2Parent;
    [SerializeField]
    UnityEngine.UI.Text ModeName;
    [Header("Config 1 for Alt Function")]
    [SerializeField]
    GameObject Config3Parent;
    [SerializeField]
    UnityEngine.UI.Text FunctionText;


    [Space(20)]

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


    int SecondaryDisplayConfig;
    bool HaveSecondary
    { get { return SecondaryDisplayConfig != 0; }  }
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

    private void UpdateSecondary(string Text, float FillPercentage)
    {
        if (SecondaryDisplayConfig == 1)
        {
            AmmoSecondary.text = Text;
            FillbarSecondary.fillAmount = FillPercentage;
        }
        else if (SecondaryDisplayConfig == 2)
        {
            ModeName.text = Text;
        }
        else if (SecondaryDisplayConfig == 3)
        {
            FunctionText.text = Text;
        }


    }

    private void SetSecondaryAvaliable(bool Avaliable)
    {
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

            SecondaryDisplayConfig = _Equipment.GetSecondaryDisplayConfig;

            SetSecondaryDisplayConfig(SecondaryDisplayConfig);

            if (HaveSecondary)
                FillbarSecondary.color = SecondaryColor;


            ECWarning.SetTrigger("EquipmentChanged");
        }
    }

    private void SetSecondaryDisplayConfig(int a)
    {
        if (a == 0)
        {
            SetSecondaryAvaliable(false);
            return;
        }

        SetSecondaryAvaliable(true);

        Config1Parent.SetActive(false);
        Config2Parent.SetActive(false);
        Config3Parent.SetActive(false);

        if (a == 1)
            Config1Parent.SetActive(true);
        else if (a == 2)
            Config2Parent.SetActive(true);
        else if (a == 3)
            Config3Parent.SetActive(true);
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
