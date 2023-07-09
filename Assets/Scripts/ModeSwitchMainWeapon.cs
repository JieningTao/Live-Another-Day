using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeSwitchMainWeapon : BaseMainSlotEquipment
{

    [SerializeField]
    protected string MainWeaponSN = "N-000";
    [SerializeField]
    protected string MainWeaponName = "Null";
    [SerializeField]
    protected Color MainWeaponGaugeColor;
    [Tooltip("Leave as 0 for any weapon except locking ones")]
    [SerializeField]
    int MainLockNum = 0;
    [SerializeField]
    int MainLockBurstAmount = 1;
    [Serializable]
    protected class FireMode
    {
        [SerializeField]
        public BaseShoot ModeShootScript;
        [SerializeField]
        public string ModeTitle;
    }
    [SerializeField]
    protected List<FireMode> FireModes = new List<FireMode>();
    protected int CurrentFireMode = 0;

    protected bool MainAmmoWarning = false;
    protected bool MainEnergyWarning = false;

    protected bool Locking = false;



    public override void PrimaryFire(bool Fire)
    {
        FireModes[CurrentFireMode].ModeShootScript.Trigger(Fire);
    }

    public override void SecondaryFire(bool Fire)
    {
        if (Fire)
            SwitchFireMode();
    }

    public override void Equip(bool _Equip, BaseMechMain _Operator, bool Right)
    {
        base.Equip(_Equip, _Operator, Right);

        foreach (FireMode a in FireModes)
        {
            a.ModeShootScript.EquipWeapon();
            if (a.ModeShootScript is BaseEnergyShoot)
                (a.ModeShootScript as BaseEnergyShoot).GetPowerSource(_Operator);
        }
    }

    public override void OperatorInit()
    {
        base.OperatorInit();

        foreach (FireMode a in FireModes)
        {
            if(a.ModeShootScript is BaseKineticShoot)
                KineticWeaponInitAmmo(a.ModeShootScript as BaseKineticShoot);
        }

    }

    public void KineticWeaponInitAmmo(BaseKineticShoot a)
    {
        if (a.GetAmmoIdentifier != "")
        {
            AttributeManager.ExtraAttribute EA = Operator.FetchExtraAttribute(a.GetAmmoIdentifier);
            //Debug.Log(EA.AttributeName);
            if (EA != null)
                a.SetAttributeExtraAmmo(EA.TributeAmount);
        }
    }

    private void SwitchFireMode()
    {
        if (CurrentFireMode < FireModes.Count - 1)
            CurrentFireMode++;
        else
            CurrentFireMode = 0;
    }

    protected virtual void CheckWarnings()
    {
        if (FireModes[0].ModeShootScript.LowAmmoWarning() && !MainAmmoWarning)
            Operator.SetWeaponWarning(Right, true, true, true);
        else if (!FireModes[0].ModeShootScript.LowAmmoWarning() && MainAmmoWarning)
            Operator.SetWeaponWarning(Right, true, true, false);

        MainAmmoWarning = FireModes[0].ModeShootScript.LowAmmoWarning();

        if (FireModes[0].ModeShootScript.LowEnergyWarning() && !MainEnergyWarning)
            Operator.SetWeaponWarning(Right, true, false, true);
        else if (!FireModes[0].ModeShootScript.LowEnergyWarning() && MainEnergyWarning)
            Operator.SetWeaponWarning(Right, true, false, false);

        MainEnergyWarning = FireModes[0].ModeShootScript.LowEnergyWarning();

    }

    protected virtual void Update()
    {
        if (Operator)
            CheckWarnings();
    }

    public override float GetBulletSpeed()
    {
        return FireModes[CurrentFireMode].ModeShootScript.GetProjectileSpeed();
    }

    public override void GetInitializeDate(out string MainFunction, out Color MainColor, out string SecondaryFunction, out Color SecondaryColor)
    {
        MainFunction = MainWeaponSN + "\n" + MainWeaponName;
        MainColor = MainWeaponGaugeColor;

        SecondaryFunction = "";
        SecondaryColor = Color.black;
    }

    public override void GetUpdateData(bool Main, out float BarFillPercentage, out string TextDisplay)
    {

        BarFillPercentage = 0;
        TextDisplay = "";

        if (Main)
        {
            BarFillPercentage = FireModes[0].ModeShootScript.GetAmmoGauge();
            TextDisplay = FireModes[0].ModeShootScript.GetAmmoText();
        }
        else
        {
            BarFillPercentage = 0;
            TextDisplay = FireModes[CurrentFireMode].ModeTitle;
        }
    }

    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();

        Temp.Add("Damage: ");
        Temp.Add(FireModes[CurrentFireMode].ModeShootScript.GetDamage);

        Temp.Add("Accuracy: ");
        Temp.Add(FireModes[CurrentFireMode].ModeShootScript.GetAccuracy);

        Temp.Add("Fire Rate: ");
        Temp.Add(FireModes[CurrentFireMode].ModeShootScript.GetFireRate);

        Temp.Add("Fire Mode: ");
        Temp.Add(FireModes[CurrentFireMode].ModeShootScript.GetFireMode);

        if (FireModes[CurrentFireMode].ModeShootScript is BaseKineticShoot)
        {
            Temp.Add("Magazine: ");
            Temp.Add(FireModes[CurrentFireMode].ModeShootScript.GetMag);

            Temp.Add("Reload: ");
            Temp.Add(FireModes[CurrentFireMode].ModeShootScript.GetReload);
        }
        else if (FireModes[CurrentFireMode].ModeShootScript is BaseEnergyShoot)
        {
            Temp.Add("Charge: ");
            Temp.Add(FireModes[CurrentFireMode].ModeShootScript.GetMag);

            Temp.Add("Recharge: ");
            Temp.Add(FireModes[CurrentFireMode].ModeShootScript.GetReload);
        }



        return Temp;
    }

    public override int GetSecondaryDisplayConfig
    { get { return 2; } }
}
