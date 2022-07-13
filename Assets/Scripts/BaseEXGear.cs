using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEXGear : MonoBehaviour
{
    protected BaseMechMain MyMech;
    [Tooltip("Displacement")]
    [SerializeField]
    protected Vector3 Displacement = Vector3.zero;
    [SerializeField]
    bool Mirrored = false;
    [SerializeField]
    protected Sprite EXGearSprite;
    [SerializeField]
    protected string EXGearName;
    [SerializeField]
    protected bool Passive = false;

    protected bool Equipped = false;
    protected BaseEnergySource MechEnergySystem;

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
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            Displacement.x *= -1;
        }
        transform.localPosition = Displacement;
    }

    public virtual bool EXGIsPassive()
    {
        return !Passive;
    }

    public virtual void TriggerGear(bool Down)
    {
        
    }

    public virtual void Equip(bool a)
    {
        Equipped = a;
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
