using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGBatteryPack : BaseEXGear
{
    [SerializeField]
    List<Rigidbody> Packs;
    [SerializeField]
    List<ParticleSystem> EjectEffects;
    [SerializeField]
    float ChargePerPack = 10;
    [SerializeField]
    float ChargeTime = 0;
    [SerializeField]
    Vector3 EjectionForce = new Vector3(0,0,-3);
    [SerializeField]
    float WeightPerTank = 0.1f;


    protected int NextChargeCount;
    protected BaseEnergySource MyEnergySource;
    [SerializeField]
    protected float ChargeTimeRemaining;
    protected bool Charging;
    protected float ChargePerSecond;

    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);
        MyEnergySource = Mech.GetEnergySystem();
        NextChargeCount = 0;

        if (ChargeTime > 0)
            ChargePerSecond = ChargePerPack / ChargeTime;

    }

    protected override void Update()
    {
        if (Charging)
        {
            ChargeTimeRemaining -= Time.deltaTime;

            if (ChargeTimeRemaining <= 0)
            {
                EjectPack();
                Charging = false;
                Charge(false);
            }

        }
        base.Update();
    }

    public void EjectPack()
    {
        //Debug.Log("Ejecting tank "+NextChargeCount);
        //Debug.Log(Packs[NextChargeCount]);
        //Debug.Log(EjectEffects[NextChargeCount]);
        if (Packs.Count > NextChargeCount)
        {
            Packs[NextChargeCount].isKinematic = false;
            Packs[NextChargeCount].transform.parent = null;
            EjectEffects[NextChargeCount].Play();

            Packs[NextChargeCount].AddForce(-Packs[NextChargeCount].transform.TransformDirection(EjectionForce), ForceMode.Impulse);
            Destroy(Packs[NextChargeCount].gameObject, 5);
            NextChargeCount++;

            MyMech.DecreaseWeight(WeightPerTank);
        }
    }

    public override float GetReadyPercentage()
    {
        return (1.0f - ((float)NextChargeCount / (float)Packs.Count));
    }

    public override float GetSubReadyPercentage()
    {
        return ChargeTimeRemaining / ChargeTime;
    }

    public override string GetBBMainText()
    {
        if (NextChargeCount < Packs.Count)
        {
            return Packs.Count - NextChargeCount + " Charges";
        }
        else
            return "EMPTY";
    }

    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);



        if (Down)
        {
            if (ChargeTimeRemaining <= 0 && NextChargeCount < Packs.Count)
            {
                if (ChargeTime == 0)
                {
                    MyEnergySource.RestoreEnergy(ChargePerPack);
                    EjectPack();
                }
                else
                {
                    Charging = true;
                    Charge(true);
                    ChargeTimeRemaining = ChargeTime;
                }
            }
        }
    }

    private void Charge(bool Start)
    {
        if (Start)
        {
            MyEnergySource.CurrentPowerDraw += ChargePerSecond;
        }
        else
            MyEnergySource.CurrentPowerDraw -= ChargePerSecond;
    }

    public override string GetBBSubText()
    {
        if (ChargeTimeRemaining > 0)
            return "Current Tank\n" + (ChargePerSecond * ChargeTimeRemaining).ToString("F1");
        else if (NextChargeCount < Packs.Count)
            return "Current Tank\n" + ChargePerPack;
        else
            return " Tank Empty";
    }

    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();

        Temp.Add("Per Tank: ");
        Temp.Add(ChargePerPack + "");

        Temp.Add("Packs: ");
        Temp.Add(Packs.Count + "");

        Temp.Add("Apply Time:");
        Temp.Add(ChargeTime + "");

        return Temp;
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Rigidbody a in Packs)
        {
            if(a!=null)
            Debug.DrawRay(a.transform.position, a.transform.TransformDirection(EjectionForce)*0.5f, Color.cyan);
        }
                
                
    }
}
