using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DamageSystem
{
    public enum DamageType
    {
        Null,
        Kinetic,
        Energy,
        DebugTest
    }

    public enum DamageTag
    {
        //KE(Kinetic Exclusive)
        //EE(Energy Exclusive)

        Chemical,
        SmallCaliber,
        Fragmentation, //KE
        Explosion,
        AP,
        Heat, //KE
        Flux,
        Cold, //EE
        Plasma, //EE

    }

    public enum ArmorType
    {
        NormSteelArmor,
        HeavyCompositeArmor,// (weight+ damage- AP Resistance- Small Caliber---)
        EnergyShield,//(Flux resistance-)
        A_Alloy,//(Kinetic Resistance+ Energy Resistance-)
        B_Alloy,//(Kinetic Resistance- Energy Resistance+) *collide with ABC

        K_Alloy,//(heat resistant+)
        W_Alloy,//(Cold resistant+)
        S_Alloy,//(Every Resistance+ Chemical Resistance--)


    }

    public enum DefensiveCoating
    {
        NoCoating,
        ABCoating,//(Energy Resistance+)
        ThermalCoating, // (HeatR+ ColdR+)
    }




    public static float GetDamageMultiplier(ArmorType HitSide, DefensiveCoating HitSideCoating, DamageType HitType, List<DamageTag> DamageTags)
    {
        if (HitType == DamageType.DebugTest)
            return 1;

        float TypeMultiplier = GetTypeMultiplier(HitSide, HitSideCoating, HitType);
        float TagMultiplier = GetTagMultiplier(HitSide, HitSideCoating, DamageTags);


        return TypeMultiplier * TagMultiplier;
    }

    private static float GetTypeMultiplier(ArmorType HitSide, DefensiveCoating HitSideCoating, DamageType HitType)
    {
        float TypeMultiplier = 1;

        if (HitType == DamageType.Energy)
        {
            if (HitSideCoating == DefensiveCoating.ABCoating)
                TypeMultiplier -= 0.2f;

            if (HitSide == ArmorType.A_Alloy)
                TypeMultiplier -= 0.3f;
            else if (HitSide == ArmorType.B_Alloy)
                TypeMultiplier += 0.3f;
            
        }
        else if (HitType == DamageType.Kinetic)
        {
            if (HitSide == ArmorType.A_Alloy)
                TypeMultiplier += 0.3f;
            else if (HitSide == ArmorType.B_Alloy)
                TypeMultiplier -= 0.3f;
        }







        return TypeMultiplier;
    }

    private static float GetTagMultiplier(ArmorType HitSide, DefensiveCoating HitSideCoating, List<DamageTag> DamageTags)
    {
        float TagMultiplier = 1;

        switch (HitSide)
        {
            case ArmorType.HeavyCompositeArmor:

                if (DamageTags.Contains(DamageTag.AP))
                    TagMultiplier += 0.5f;
                else if (DamageTags.Contains(DamageTag.SmallCaliber) || DamageTags.Contains(DamageTag.Fragmentation))
                    TagMultiplier -= 0.8f;
                else
                    TagMultiplier -= 0.3f;
                break;

            case ArmorType.EnergyShield:
                if (DamageTags.Contains(DamageTag.Flux))
                    TagMultiplier += 0.5f;
                break;

            case ArmorType.NormSteelArmor:
                if (DamageTags.Contains(DamageTag.Plasma))
                    TagMultiplier += 0.25f;
                break;

        }

        if (HitSideCoating == DefensiveCoating.ThermalCoating)
        {
            if (DamageTags.Contains(DamageTag.Chemical))
                TagMultiplier += 0.25f;
            else if (DamageTags.Contains(DamageTag.Heat) || DamageTags.Contains(DamageTag.Cold))
                TagMultiplier -= 0.3f;
        }




        return TagMultiplier;
    }







}
