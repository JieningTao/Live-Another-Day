using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoostSystem :MonoBehaviour
{
    [SerializeField]
    protected float BoostForce = 270;
    [SerializeField]
    protected float ImpulseBoostForce = 90;
    [SerializeField]
    protected float FloatForce = 35;

    [Space(15)]

    [SerializeField]
    protected float BoostCost = 4;
    [SerializeField]
    protected float ImpulseBoostCost = 10;
    [SerializeField]
    protected float FloatCost = 6;

    [Space(15)]

    [SerializeField]
    protected float BoostCapacity;
    [SerializeField]
    protected float BoostRecovery;
    [SerializeField]
    protected float BoostRecoverCooldown;

    [Space(15)]

    [SerializeField]
    protected GameObject BoostExhaust;
    [SerializeField]
    protected GameObject ImpulseBoost;
    [SerializeField]
    protected GameObject FloatThrust;


    public void OutStats(out float BF, out float IBF, out float FF, out float BC, out float IBC, out float FC, out float BJC, out float BJR, out float BJRC)
    {
        BF = BoostForce;
        IBF = ImpulseBoostForce;
        FF = FloatForce;

        BC = BoostCost;
        IBC = ImpulseBoostCost;
        FC = FloatCost;

        BJC = BoostCapacity;
        BJR = BoostRecovery;
        BJRC = BoostRecoverCooldown;
    }

    public GameObject GetBoostExhaust()
    {
        return BoostExhaust;
    }

    public GameObject GetImpulseBoost()
    {
        return ImpulseBoost;
    }

    public GameObject GetFloatThrust()
    {
        return FloatThrust;
    }


}
