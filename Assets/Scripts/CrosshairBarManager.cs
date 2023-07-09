using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairBarManager : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Image LeftBar;
    [SerializeField]
    UnityEngine.UI.Image RightBar;
    [SerializeField]
    UnityEngine.UI.Image BottomBar;

    [SerializeField]
    Animator CenterHitMarker;
    [SerializeField]
    Animator LeftHitMark;
    [SerializeField]
    UnityEngine.UI.Text LeftDamagemark;
    [SerializeField]
    Animator RightHitMark;
    [SerializeField]
    UnityEngine.UI.Text RightDamagemark;


    BaseMainSlotEquipment LeftEquipment;
    BaseMainSlotEquipment RightEquipment;
    BaseEXGear EXG;


    private void Update()
    {
        if (LeftEquipment)
        {
            LeftEquipment.GetUpdateData(true, out float LFP, out string TD);
            LeftBar.fillAmount = LFP;
        }

        if (RightEquipment)
        {
            RightEquipment.GetUpdateData(true, out float RFP, out string TD);
            RightBar.fillAmount = RFP;
        }

        if (EXG)
        {
            BottomBar.fillAmount = EXG.GetReadyPercentage();
        }
    }

    private void NewMainHandEquipment(bool Right, BaseMainSlotEquipment _Equipment)
    {
        if (Right)
        {
            RightEquipment = _Equipment;


            if (_Equipment)
            {
                RightBar.gameObject.SetActive(true);
                _Equipment.GetInitializeDate(out string MainName, out Color MainColor, out string SecondaryName, out Color SecondaryColor);
                RightBar.color = MainColor;
            }
            else
                RightBar.gameObject.SetActive(false);
        }
        else
        {
            LeftEquipment = _Equipment;
            if (_Equipment)
            {
                LeftBar.gameObject.SetActive(true);
                _Equipment.GetInitializeDate(out string MainName, out Color MainColor, out string SecondaryName, out Color SecondaryColor);
                LeftBar.color = MainColor;
            }
            else
                LeftBar.gameObject.SetActive(false);
        }

    



    }

    private void NewEXG(int a, string b, BaseEXGear c)
    {



        if (b == "Select")
        {
            //Debug.Log(c);

            EXG = c;

            if (c)
                BottomBar.gameObject.SetActive(true);
            else
                BottomBar.gameObject.SetActive(false);
        }


        //if (b == "New")
        //{
        //    AllEXGs[a - 1] = c;
        //}

        //else if (b == "Select")
        //{
        //    CurrentSelectedEXG = AllEXGs[a - 1];
        //    Title.text = CurrentSelectedEXG.GetName();
        //}
    }

    private void HandleHitMarks(IDamageable Damaged,string Type,float Damage, IDamageSource Source)
    {
        if (Type == "Damage")
        {
            //Debug.Log(Source);
            //Debug.Log(LeftEquipment);
            //Debug.Log((Object)Source == LeftEquipment);

            if ((Object)Source == LeftEquipment)
            {
                LeftHitMark.SetTrigger("Hit");
                LeftDamagemark.text = Damage + "";
                CenterHitMarker.SetTrigger("Hit");
            }
            else if ((Object)Source == RightEquipment)
            {
                RightHitMark.SetTrigger("Hit");
                RightDamagemark.text = Damage + "";
                CenterHitMarker.SetTrigger("Hit");
            }
            else if ((Object)Source == EXG)
            {
                CenterHitMarker.SetTrigger("Hit");
            }
        }


    }

    private void OnEnable()
    {
        //BaseMechFCS.WeaponWarnings += WeaponWarnings;
        BaseMechFCS.WeaponChanges += NewMainHandEquipment;
        BaseMechFCS.EXGearChanges += NewEXG;
        IDamageable.DamageablePing += HandleHitMarks;
    }

    private void OnDisable()
    {
        //BaseMechFCS.WeaponWarnings -= WeaponWarnings;
        BaseMechFCS.WeaponChanges -= NewMainHandEquipment;
        BaseMechFCS.EXGearChanges -= NewEXG;
        IDamageable.DamageablePing -= HandleHitMarks;
    }
}
