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
    float EjectionForce =3;

    protected int NextChargeCount;

    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);

        NextChargeCount = 0;
    }

    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);


        if (Tanks.Count > NextChargeCount)
        {
            Tanks[NextChargeCount].isKinematic = false;
            Tanks[NextChargeCount].transform.parent = null;
            EjectEffects[NextChargeCount].Play();

            Tanks[NextChargeCount].AddForce(-Tanks[0].transform.forward * EjectionForce, ForceMode.Impulse);
            Destroy(Tanks[NextChargeCount].gameObject, 5);
            NextChargeCount++;
        }





    }

    public override float GetReadyPercentage()
    {
        return 1 - NextChargeCount/Tanks.Count;
    }

}
