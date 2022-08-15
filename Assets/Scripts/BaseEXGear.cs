using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEXGear : MonoBehaviour
{
    [Tooltip("Displacement")]
    [SerializeField]
    protected Vector3 Displacement = Vector3.zero;
    [SerializeField]
    protected float Weight;
    [SerializeField]
    protected bool Mirrored = false;
    [SerializeField]
    protected Sprite EXGearSprite;
    [SerializeField]
    protected string EXGearName;
    [SerializeField]
    protected bool Passive = false;
    [SerializeField]
    protected Animator MyAnimator;
    [SerializeField]
    protected float ReadyTime;


    protected BaseMechMain MyMech;
    protected bool Equipped = false;
    protected BaseEnergySource MechEnergySystem;
    protected float ReadyTimer;

    public virtual void InitializeGear(BaseMechMain Mech,Transform Parent,bool Right)
    {
        if (Parent)
            transform.parent = Parent;

        MyMech = Mech;
        MechEnergySystem = Mech.GetEnergySystem();
        gameObject.layer = Mech.gameObject.layer;

        if (!Right)
        {
            if (Mirrored)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); // this triggered twice and caused it to mirror twice and be back unmirrored
                Debug.Log("Mirrored", this);
            }

            Displacement.x *= -1;
        }
        transform.localPosition = Displacement;
    }

    public virtual bool EXGIsPassive()
    {
        return !Passive;
    }

    public virtual float GetWeight()
    {
        return Weight;
    }


    public virtual void TriggerGear(bool Down)
    {
        if (ReadyTimer > 0)
            return;
    }

    protected virtual void Update()
    {
        if (Equipped && ReadyTime > 0)
            ReadyTimer -= Time.deltaTime;
    }

    public virtual void Equip(bool a)
    {
        Equipped = a;

        if (MyAnimator)
            MyAnimator.SetBool("Deployed", a);

        ReadyTimer = ReadyTime;
    }

    public virtual void GetInitializeData(out Sprite EXGSprite, out string EXGName)
    {
        EXGSprite = EXGearSprite;
        EXGName = EXGearName;
    }

    public virtual float GetReadyPercentage()
    {
        return 1;
    }

    public virtual void ReSupply(float Percentage)
    {

    }
}
