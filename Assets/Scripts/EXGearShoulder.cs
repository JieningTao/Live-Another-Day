using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXGearShoulder : BaseEXGear
{

    [SerializeField]
    float ReadyTime;
    [SerializeField]
    protected Animator MyAnimator;

    protected float ReadyTimer;

    protected virtual void Update()
    {
        if (Equipped && ReadyTime > 0)
            ReadyTimer -= Time.deltaTime;
    }

    public override void TriggerGear(bool Down)
    {
        if (ReadyTimer > 0)
            return;
    }

    public override void Equip(bool a)
    {
        base.Equip(a);

        if (MyAnimator)
            MyAnimator.SetBool("Deployed", a);

        ReadyTimer = ReadyTime;
    }



}
