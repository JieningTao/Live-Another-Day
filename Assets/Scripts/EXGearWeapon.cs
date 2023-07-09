using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearWeapon : BaseEXGear
{
    [SerializeField]
    protected BaseShoot MyWeapon;
    [Tooltip("random effects to be used with events in animation, leave blank if none")]
    [SerializeField]
    List<ParticleSystem> MiscEffects = new List<ParticleSystem>();

    protected BaseMechFCS MyFCS;


    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);

        MyFCS = Mech.GetFCS();

        if (MyWeapon is BaseEnergyShoot)
        {
            (MyWeapon as BaseEnergyShoot).GetPowerSource(Mech);
            //Debug.Log("Is E");
        }

        if (MyWeapon is BaseKineticShoot)
        {
            KineticWeaponInitAmmo(MyWeapon as BaseKineticShoot);
        }
        //Debug.Log(gameObject.name + " Right: " + Right, this);
    }

    public override void Equip(bool a)
    {
        base.Equip(a);
        TriggerGear(false);
    }

    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down); //base trigger considers ready time and returns if not ready
        if (ReadyTimer > 0)
            return;

        MyWeapon.Trigger(Down);
    }


    public void KineticWeaponInitAmmo(BaseKineticShoot a)
    {
        if (a.GetAmmoIdentifier != "")
        {
            AttributeManager.ExtraAttribute EA = MyFCS.FetchExtraAttribute(a.GetAmmoIdentifier);
            //Debug.Log(EA.AttributeName);
            if (EA != null)
                a.SetAttributeExtraAmmo(EA.TributeAmount);

        }
    }

    public override float GetReadyPercentage()
    {
        return MyWeapon.GetAmmoGauge();
    }

    public override string GetBBMainText()
    {
        return MyWeapon.GetAmmoText();
    }

    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();

        Temp.Add("Damage: ");
        Temp.Add(MyWeapon.GetDamage);

        Temp.Add("Accuracy: ");
        Temp.Add(MyWeapon.GetAccuracy);

        Temp.Add("Fire Rate: ");
        Temp.Add(MyWeapon.GetFireRate);

        Temp.Add("Fire Mode: ");
        Temp.Add(MyWeapon.GetFireMode);

        if (MyWeapon is BaseKineticShoot)
        {
            Temp.Add("Magazine: ");
            Temp.Add(MyWeapon.GetMag);

            Temp.Add("Reload: ");
            Temp.Add(MyWeapon.GetReload);
        }
        else if (MyWeapon is BaseEnergyShoot)
        {
            Temp.Add("Charge: ");
            Temp.Add(MyWeapon.GetMag);

            Temp.Add("Recharge: ");
            Temp.Add(MyWeapon.GetReload);
        }

        return Temp;
    }

    public override bool IsAimed
    {
        get { return false; }
    }

    public void PlayeEffect(int EffectNum)
    {
        try
        {
            MiscEffects[EffectNum].Play();
        }
        catch
        {
            Debug.Log("EffectError", this);
        }
    }
}
