using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHandShield : BaseMainSlotEquipment
{
    [SerializeField]
    protected PhysicalShield Shield;
    [SerializeField]
    protected string GearSN = "N-000";
    [SerializeField]
    protected string GearName = "Null";
    [SerializeField]
    protected Color GearGaugeColor;

    protected BaseMechFCS MyFCS;



    public override void Equip(bool _Equip, BaseMechMain Operator, bool Right)
    {
        base.Equip(_Equip, Operator, Right);
        if (_Equip)
        {
            if(Operator)
            MyFCS = Operator.GetFCS();
            Shield.Equip(Operator);
        }
        else
        {
            MyFCS = null;
            Shield.Equip(null);
        }
        //needs to flip game object if on right hand
        //if ()
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

        return Temp;
    }

    private void OnDestroy()
    {
        if(MyFCS)
        MyFCS.UnEquip(this);   
    }

}
