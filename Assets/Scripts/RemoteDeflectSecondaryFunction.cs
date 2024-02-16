using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteDeflectSecondaryFunction : BaseSecondaryMainWeaponFunction
{
    [SerializeField]
    protected BaseShoot ShootSource;
    [SerializeField]
    protected string FunctionName;
    [SerializeField]
    protected float FunctionCD;
    protected float FunctionCDRemaining;

    BaseMechFCS FCS;

    public override void InitFunction(BaseMechMain Operator)
    {
        FCS = Operator.GetFCS();
    }

    private void Update()
    {
        if (FunctionCDRemaining > 0)
            FunctionCDRemaining -= Time.deltaTime;
    }

    public override void Trigger(bool Down)
    {
        if (Down&&FunctionReady)
        {
            RemoteWeaponaryEvents.InvokeRWS(ShootSource, "SnapTowards", FCS.GetMainTarget());
            FunctionCDRemaining = FunctionCD;
        }
    }

    public override bool FunctionReady
    { get { return FunctionCDRemaining<=0; } }

    public override string UpdateText
    { get {
            if (FunctionReady)
                return FunctionName;
            else
                return "CD " + FunctionCDRemaining.ToString("F1") + "s";
        } }
}
