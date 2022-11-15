using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHandEnergyShield : BaseMainSlotEquipment
{
    [SerializeField]
    EnergyShield Shield;
    [SerializeField]
    protected string GearSN = "N-000";
    [SerializeField]
    protected string GearName = "Null";
    [SerializeField]
    protected Color GearGaugeColor;




    public override void PrimaryFire(bool Fire)
    {
        Shield.ToggleShield(Fire);
    }

    public override void Equip(bool _Equip, BaseMechMain Operator,bool Right)
    {
        base.Equip(_Equip,Operator,Right);
        Shield.Equip(Operator);
    }

    public override void GetInitializeDate(out string MainFunction, out Color MainColor, out string SecondaryFunction, out Color SecondaryColor)
    {
        MainFunction = GearSN + "\n" + GearName;
        MainColor = GearGaugeColor;
        SecondaryFunction = "";
        SecondaryColor = Color.black;
    }

    public override void GetUpdateData(bool Main, out float BarFillPercentage, out string TextDisplay)
    {
        BarFillPercentage = Shield.GetHealthPercent();
        TextDisplay = Shield.GetHealthText();
    }


    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();

        Temp.Add("HP:");
        Temp.Add(Shield.GetMaxHealth);

        Temp.Add("Recharge:");
        Temp.Add(Shield.GetRechargeRate+"--"+Shield.GetRechargeDelay);

        Temp.Add("Active Charge:");
        Temp.Add(Shield.GetChargeWhileDeployed);

        Temp.Add("Overload Time:");
        Temp.Add(Shield.GetOverLoadRecharge);

        Temp.Add("Charge Power:");
        Temp.Add(Shield.GetChargePowerDraw);

        Temp.Add("Deployed Power:");
        Temp.Add(Shield.GetDeployedPowerDraw);

        return null;
    }
}
