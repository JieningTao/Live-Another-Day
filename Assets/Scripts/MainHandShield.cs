﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHandShield : BaseMainSlotEquipment
{
    [SerializeField]
    PhysicalShield Shield;
    [SerializeField]
    protected string GearSN = "N-000";
    [SerializeField]
    protected string GearName = "Null";
    [SerializeField]
    protected Color GearGaugeColor;



    public override void Equip(bool _Equip, BaseMechMain Operator)
    {
        gameObject.layer = Operator.gameObject.layer;
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
}