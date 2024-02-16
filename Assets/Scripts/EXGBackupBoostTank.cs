using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGBackupBoostTank : BaseEXGear
{
    [SerializeField]
    List<Rigidbody> Tanks;
    [SerializeField]
    List<ParticleSystem> EjectEffects;
    [SerializeField]
    float ChargePerTank = 10;
    [SerializeField]
    float ChargeTime = 0;
    [SerializeField]
    float EjectionForce =3;
    [SerializeField]
    float WeightPerTank =0.1f;

    protected int NextChargeCount;
    protected BaseMechMovement MyMovement;
    [SerializeField]
    protected float ChargeTimeRemaining;
    protected bool Charging;
    protected float ChargePerSecond;


    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);
        MyMovement = Mech.GetMovement();
        NextChargeCount = 0;

        if(ChargeTime>0)
        ChargePerSecond = ChargePerTank / ChargeTime;

    }


    protected override void Update()
    {
        if (Charging)
        {
            if (Time.deltaTime < ChargeTimeRemaining)
            {
                ChargeTimeRemaining -= Time.deltaTime;
                MyMovement.RestoreBoostJuice(ChargePerSecond * Time.deltaTime);

            }
            else
            {
                MyMovement.RestoreBoostJuice(ChargePerSecond * ChargeTimeRemaining);
                ChargeTimeRemaining = 0;
                EjectTank();
                Charging = false;
            }
        }
        base.Update();
    }


    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);

        

        if (Down)
        {
            if (ChargeTimeRemaining <= 0 && NextChargeCount < Tanks.Count)
            {
                if (ChargeTime == 0) // the pack is instant
                {
                    MyMovement.RestoreBoostJuice(ChargePerTank);
                    EjectTank();
                }
                else
                {
                    Charging = true;
                    ChargeTimeRemaining = ChargeTime;
                }
            }
        }
    }

    public void EjectTank()
    {
        //Debug.Log("Ejecting tank "+NextChargeCount);
        //Debug.Log(Tanks[NextChargeCount]);
        //Debug.Log(EjectEffects[NextChargeCount]);
        if (Tanks.Count > NextChargeCount)
        {
            Tanks[NextChargeCount].isKinematic = false;
            Tanks[NextChargeCount].transform.parent = null;
            EjectEffects[NextChargeCount].Play();

            Tanks[NextChargeCount].AddForce(-Tanks[NextChargeCount].transform.forward * EjectionForce, ForceMode.Impulse);
            Destroy(Tanks[NextChargeCount].gameObject, 5);
            NextChargeCount++;

            MyMech.DecreaseWeight(WeightPerTank);
        }
    }

    public override float GetReadyPercentage()
    {
        return (1.0f -( (float)NextChargeCount/(float)Tanks.Count));
    }

    public override float GetSubReadyPercentage()
    {
        return ChargeTimeRemaining/ChargeTime;
    }


    public override string GetBBMainText()
    {
        if (NextChargeCount < Tanks.Count)
        {
            return Tanks.Count - NextChargeCount + " Charges";
        }
        else
        return "EMPTY";
    }

    public override string GetBBSubText()
    {
        if(ChargeTimeRemaining>0)
        return "Current Tank\n" + (ChargePerSecond * ChargeTimeRemaining).ToString("F1");
        else if(NextChargeCount < Tanks.Count)
            return "Current Tank\n" + ChargePerTank;
        else
            return " Tank Empty";
    }

    public override List<string> GetStats()
    {
        List<string> Temp = new List<string>();

        Temp.Add("Per Tank: ");
        Temp.Add(ChargePerTank+"");

        Temp.Add("Tanks: ");
        Temp.Add(Tanks.Count+"");

        Temp.Add("Apply Time:");
        Temp.Add(ChargeTime+"");

        return Temp;
    }
}
