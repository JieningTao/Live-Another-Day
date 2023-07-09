using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEXGear : MonoBehaviour,IDamageSource
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
    [SerializeField]
    protected float ReadyTimer;

    public virtual void InitializeGear(BaseMechMain Mech,Transform Parent,bool Right)
    {
        PositionGear(Parent, Right);

        MyMech = Mech;
        if (Mech)
        {
            MechEnergySystem = Mech.GetEnergySystem();
            gameObject.layer = Mech.gameObject.layer;
        }

    }

    public virtual void PositionGear(Transform Parent, bool Right)
    {
        if (Parent)
            transform.parent = Parent;

        if (!Right)
        {
            if (Mirrored)
            {
                if (transform.localScale.x > 0)
                {
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z); // this triggered twice and caused it to mirror twice and be back unmirrored

                }

                //Displacement.x = Mathf.Abs(Displacement.x);
                //Debug.Log("Mirrored", this);
            }
                Displacement.x = -Mathf.Abs(Displacement.x);
        }
        transform.localPosition = Displacement;

        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public virtual bool EXGIsPassive
    { get { return Passive; } }

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

    public virtual string GetName()
    {
        return EXGearName;
    }

    public virtual float GetReadyPercentage()
    {
        return 1;
    }

    public virtual float GetSubReadyPercentage()
    {
        return 0;
    }

    public virtual string GetBBMainText()
    {
        return "ERR";
    }

    public virtual string GetBBSubText()
    {
        return "";
    }

    public virtual void ReSupply(float Percentage)
    {

    }

    public virtual List<string> GetStats()
    {
        return null;
    }

    public IDamageSource DamageSource()
    {
        return this;
    }

    public virtual bool IsAimed
    {
        get { return false; }
    }
}
