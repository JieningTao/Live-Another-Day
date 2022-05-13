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
        ABCoating,//(Absorbs 80% energy damage)
        ThermalCoating, // (absorbs 50% heat and cold damage)
    }


    public static float GetCoatingAbsorbRate(DefensiveCoating HitCoating, DamageType HitType, List<DamageTag> DamageTags)
    {
        switch (HitCoating)
        {
            default:
            case DefensiveCoating.NoCoating:
                return 0;
                break;
            case DefensiveCoating.ABCoating:
                if (HitType == DamageType.Energy)
                    return 0.8f;
                else
                    return 0;
                break;
            case DefensiveCoating.ThermalCoating:
                if(DamageTags.Contains(DamageTag.Cold)||DamageTags.Contains(DamageTag.Heat))
                    return 0.5f;
                else return 0;
                break;
        }
    }

    public static float GetDamageMultiplier(ArmorType HitSide, DamageType HitType, List<DamageTag> DamageTags)
    {
        if (HitType == DamageType.DebugTest)
            return 1;

        float TypeMultiplier = GetTypeMultiplier(HitSide,  HitType);
        float TagMultiplier = GetTagMultiplier(HitSide,  DamageTags);


        return TypeMultiplier * TagMultiplier;
    }

    private static float GetTypeMultiplier(ArmorType HitSide,  DamageType HitType)
    {
        float TypeMultiplier = 1;

        if (HitType == DamageType.Energy)
        {
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

    private static float GetTagMultiplier(ArmorType HitSide,  List<DamageTag> DamageTags)
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

        return TagMultiplier;
    }







}
