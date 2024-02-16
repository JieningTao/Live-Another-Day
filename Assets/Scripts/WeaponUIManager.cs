using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUIManager : MonoBehaviour
{
    [SerializeField]
    UIWeaponDisplayBox RightMain;
    [SerializeField]
    UIWeaponDisplayBox RightSecondary;
    [SerializeField]
    Animator RightECWarning;

    [SerializeField]
    UIWeaponDisplayBox LeftMain;
    [SerializeField]
    UIWeaponDisplayBox LeftSecondary;
    [SerializeField]
    Animator LeftECWarning;

    private BaseMainSlotEquipment RightEquipment;
    private BaseMainSlotEquipment LeftEquipment;
    private bool RightHasSecondary;
    private bool LeftHasSecondary;

    private void Update()
    {
        UpdateWeaponStatus(true);
        UpdateWeaponStatus(false);
    }

    public void UpdateWeaponStatus(bool Right)
    {
        if (Right)
        {
            if (RightEquipment == null)
                return;

            RightEquipment.GetUpdateData(true, out float BFP, out string TD);
            RightMain.RegularUpdate(TD, BFP);

            if (RightHasSecondary)
            {
                RightEquipment.GetUpdateData(false, out float SBFP, out string STD);
                RightSecondary.RegularUpdate(STD, SBFP);
            }
        }
        else
        {
            if (LeftEquipment == null)
                return;

            LeftEquipment.GetUpdateData(true, out float BFP, out string TD);
            LeftMain.RegularUpdate(TD, BFP);

            if (LeftHasSecondary)
            {
                LeftEquipment.GetUpdateData(false, out float SBFP, out string STD);
                LeftSecondary.RegularUpdate(STD, SBFP);
            }
        }
    }

    public void NewWeapon(bool Right, BaseMainSlotEquipment Equipment)
    {
        Debug.Log(Equipment);

        UIWeaponDisplayBox TempMain;
        UIWeaponDisplayBox TempSecondary;

        if (Right)
        {
            TempMain = RightMain;
            TempSecondary = RightSecondary;
            RightEquipment = Equipment;
            RightECWarning.SetTrigger("EquipmentChanged");
            //RightECWarning.gameObject.SetActive(false);
            //RightECWarning.gameObject.SetActive(true);
        }
        else
        {
            TempMain = LeftMain;
            TempSecondary = LeftSecondary;
            LeftEquipment = Equipment;
            LeftECWarning.SetTrigger("EquipmentChanged");
            //LeftECWarning.gameObject.SetActive(false);
            //LeftECWarning.gameObject.SetActive(true);
        }

        if (!Equipment)
        {
            TempMain.SetAvaliable(false);
            TempSecondary.SetAvaliable(false);
            return;
        }


        Equipment.GetInitializeDate(out string MainName,out Color MainColor,out string SecondaryName,out Color SecondaryColor);


        TempMain.StartInitialize(MainName, MainColor,Right,true);

        if (SecondaryName != "\n"&& SecondaryName != "")
        {
            if (Right)
                RightHasSecondary = true;
            else
                LeftHasSecondary = true;
            TempSecondary.SetAvaliable(true);
            TempSecondary.StartInitialize(SecondaryName, SecondaryColor,Right,false);
            
        }
        else
        {
            if (Right)
                RightHasSecondary = false;
            else
                LeftHasSecondary = false;
            TempSecondary.SetAvaliable(false);
            //TempSecondary.StartInitialize(SecondaryName, SecondaryColor);
        }

        UpdateWeaponStatus(Right);
        
    }



    private void OnEnable()
    {
        BaseMechFCS.WeaponChanges += NewWeapon;
    }

    private void OnDisable()
    {
        BaseMechFCS.WeaponChanges -= NewWeapon;
    }

}
