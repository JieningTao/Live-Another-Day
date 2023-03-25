using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBoostSystem : MonoBehaviour
{
    [SerializeField]
    protected float BoostForce = 270;
    [SerializeField]
    protected float ImpulseBoostForce = 90;
    [SerializeField]
    protected float FloatForce = 35;
    [SerializeField]
    protected float ExtraSpeedCap = 20;

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


    protected BaseMechMovement MyBMM;
    protected List<ParticleSystem> BoostExhausts = new List<ParticleSystem>();
    protected List<ParticleSystem> BoostImpulses = new List<ParticleSystem>();
    protected List<ParticleSystem> FloatThrusters = new List<ParticleSystem>();

    protected bool Boosting;
    protected bool Floating;

    protected int LastEightDividDirection = 0;

    public void OutStats(out float BF, out float IBF, out float FF, out float ESC, out float BC, out float IBC, out float FC, out float BJC, out float BJR, out float BJRC, out BaseBoostSystem BS)
    {
        BF = BoostForce;
        IBF = ImpulseBoostForce;
        FF = FloatForce;
        ESC = ExtraSpeedCap;

        BC = BoostCost;
        IBC = ImpulseBoostCost;
        FC = FloatCost;

        BJC = BoostCapacity;
        BJR = BoostRecovery;
        BJRC = BoostRecoverCooldown;

        BS = this;
    }

    public void InitBS(BaseMechMain Mech)
    {
        MyBMM = Mech.GetMovement();
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

    public void CreateBoostAndJumpEffects(List<Transform> BoostPoints, List<Transform> FloatThrustPoints)
    {
        BoostExhausts = new List<ParticleSystem>();
        BoostImpulses = new List<ParticleSystem>();

        FloatThrusters = new List<ParticleSystem>();

        for (int i = 0; i < BoostPoints.Count; i++)
        {
            ParticleSystem Boost = Instantiate(BoostExhaust, BoostPoints[i]).GetComponentInChildren<ParticleSystem>();
            AdjustEffectScale(Boost, BoostPoints[i].transform.localScale);
            BoostExhausts.Add(Boost);

            ParticleSystem Impulse = Instantiate(ImpulseBoost, BoostPoints[i]).GetComponentInChildren<ParticleSystem>();
            AdjustEffectScale(Impulse, BoostPoints[i].transform.localScale);
            BoostImpulses.Add(Impulse);
        }
        for (int i = 0; i < FloatThrustPoints.Count; i++)
        {
            ParticleSystem Float = Instantiate(FloatThrust, FloatThrustPoints[i]).GetComponentInChildren<ParticleSystem>();
            AdjustEffectScale(Float, FloatThrustPoints[i].transform.localScale);
            FloatThrusters.Add(Float);
        }
    }


    private void Update()
    {
        if(Boosting)
            EightDivide();
    }

    public void ImpulseBoostEffect()
    {
        foreach (ParticleSystem a in BoostImpulses)
        {
            a.Play();
        }
    }

    public void ImpulseBoostEffect(Vector3 Direction)
    {
        foreach (ParticleSystem a in BoostImpulses)
        {
            //Debug.Log(Vector3.Angle(a.transform.forward, Direction));
            if (Vector3.Angle(-a.transform.forward, Direction) < 60)
                a.Play();
        }
    }

    public void BoostEffect(bool boost)
    {
        foreach (ParticleSystem a in BoostExhausts)
        {
            if (boost)
                a.Play();
            else
                a.Stop();
        }
        Boosting = boost;
    }

    public void BoostEffect(Vector3 Direction, bool boost)
    {
        foreach (ParticleSystem a in BoostExhausts)
        {
            if (boost)
            {
                if (Vector3.Angle(-a.transform.forward, Direction) < 60)
                    a.Play();
            }
            else
                a.Stop();
        }
        Boosting = boost;
    }

    public void FloatEffect(bool _float)
    {
        foreach (ParticleSystem a in FloatThrusters)
        {
            if (_float)
                a.Play();
            else
                a.Stop();
        }
        Floating = _float;
    }

    protected void EightDivide()
    {
        Vector3 PlanarDirection = new Vector3(MyBMM.MovementInput.x, 0, MyBMM.MovementInput.z);

        float Angle = Mathf.Atan2(PlanarDirection.x, PlanarDirection.z) * Mathf.Rad2Deg;

        if (Angle < 0)
            Angle = (360 + Angle);

        int EDD = (int)(Angle / 45f);

        if (LastEightDividDirection != EDD)
        {
            //boost direction is in a new eighth quadrant direction

            ToggleBoostDirection(EDDToVector3(EDD));
            LastEightDividDirection = EDD;
        }

    }

    protected Vector3 EDDToVector3(int EDD)
    {
        switch (EDD)
        {
            case 0: return new Vector3(0, 0, 1);
            case 1: return new Vector3(1, 0, 1).normalized;
            case 2: return new Vector3(1, 0, 0);
            case 3: return new Vector3(1, 0, -1).normalized;
            case 4: return new Vector3(0, 0, -1);
            case 5: return new Vector3(-1, 0, -1).normalized;
            case 6: return new Vector3(-1, 0, 0);
            case 7: return new Vector3(-1, 0, 1).normalized;
        }
        return new Vector3(0, 0, 1);
    }

    protected void ToggleBoostDirection(Vector3 Direction)
    {
        foreach (ParticleSystem a in BoostExhausts)
        {
            if (Vector3.Angle(-a.transform.forward, Direction) < 60)
                a.Play();
            else
                a.Stop();
        }
    }


    public void AdjustEffectScale(ParticleSystem a, Vector3 Scale)
    {
        foreach (ParticleSystem b in a.GetComponentsInChildren<ParticleSystem>())
        {
            b.transform.localScale = Scale;
        }
    }

    private void OnDisable()
    {
        BoostEffect(false);
        FloatEffect(false);
    }

    #region Loadoutpart request info stuff

    public virtual string GetBoostCapacity
    { get { return BoostCapacity + ""; } }

    public virtual string GetRecovery
    { get { return BoostRecovery + "/s"; } }

    public virtual string GetRecoveryCD
    { get { return BoostRecoverCooldown.ToString("F2") + "s"; } }


    public virtual string GetBoostForce
    { get { return BoostForce+""; } }

    public virtual string GetBoostCost
    { get { return BoostCost + "/s"; } }


    public virtual string GetImpulseForce
    { get { return ImpulseBoostForce+""; } }

    public virtual string GetImpulseCost
    { get { return ImpulseBoostCost + ""; } }



    public virtual string GetFloatForce
    { get { return FloatForce + ""; } }

    public virtual string GetFloatCost
    { get { return FloatCost + "/s"; } }

    #endregion
}
