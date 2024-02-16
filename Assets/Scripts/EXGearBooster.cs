using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearBooster : BaseEXGear
{
    [SerializeField]
    protected float BoostForce;
    [SerializeField]
    protected float ConsumePercentagePerSecond;
    [SerializeField]
    protected float RecoverPercentagePerSecond;
    [SerializeField]
    protected float RecoverCooldown;
    [SerializeField]
    protected ParticleSystem BoostEffect;

    float BoostPercentageRemaining;
    [SerializeField]
    Rigidbody MyRB;
    bool Boosting;
    float RecoverCD;


    public override void InitializeGear(BaseMechMain Mech, Transform Parent, bool Right)
    {
        base.InitializeGear(Mech, Parent, Right);

        MyRB = Mech.GetMovement().GetRigidbody;

        BoostPercentageRemaining = 1;
    }


    public override void TriggerGear(bool Down)
    {
        base.TriggerGear(Down);

        if (Down)
            ControlBoost(true);
        else
            ControlBoost(false);
    }

    protected override void Update()
    {
        base.Update();

        if (Boosting)
        {
            Boost();
        }
        else if (RecoverCooldown > 0)
            RecoverCooldown -= Time.deltaTime;
        else if (BoostPercentageRemaining < 1)
        {
            RecoverBoost();
        }

    }

    protected void Boost()
    {
        float Consumed = ConsumePercentagePerSecond * Time.deltaTime;

        if (BoostPercentageRemaining > Consumed)
        {
            MyRB.AddForce(transform.forward * BoostForce, ForceMode.Force);
            BoostPercentageRemaining -= Consumed;
        }
        else
            ControlBoost(false);

    }

    protected void ControlBoost(bool StartEnd)
    {
        if (StartEnd && !Boosting && BoostPercentageRemaining>0)
        {
            BoostEffect.Play();
            RecoverCD = RecoverCooldown;
            Boosting = true;
        }
        else if(!StartEnd && Boosting)
        {
            BoostEffect.Stop();
            Boosting = false;
        }
    }

    protected void RecoverBoost()
    {
        BoostPercentageRemaining += RecoverPercentagePerSecond * Time.deltaTime;
        if (BoostPercentageRemaining > 1)
            BoostPercentageRemaining = 1;
    }

    public override float GetReadyPercentage()
    {
        return BoostPercentageRemaining;
    }

}
