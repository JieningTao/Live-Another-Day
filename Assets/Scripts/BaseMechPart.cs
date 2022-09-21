using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechPart : MonoBehaviour
{
    public float Health = 1;
    public float Weight = 0.1f;

    [SerializeField]
    public List<AdditionalAttribute> Attributs = new List<AdditionalAttribute>();
    [Serializable]
    public class AdditionalAttribute
    {
        public enum Attribute
        {
            TurnSpeed,
            MoveForce,
            SpeedLimit,
            BoostMultiplier,
            BoostCost,
            ImpulseBoostForce,
            ImpulseCost,
            AmmoAmount,

            BoostJuiceCapacity,
            BoostJuiceRecovery,
            BoostJuiceRecoveryCooldown,

            JumpForce,
            FloatForce,
        }

        [SerializeField]
        public Attribute AttributeType;
        [SerializeField]
        public float TributeAmount;
    }

    public List<Transform> FloatThrusters;
    public List<Transform> BoostThrusters;

    [SerializeField]
    protected Vector3 Displacement;



    protected BaseMechMain MyMech;


    public virtual void Assemble(BaseMechMain Mech, Transform JointPosition)
    {
        VisualAssemble(JointPosition);
        MyMech = Mech;
        SetLayer(Mech.gameObject.layer);
    }

    public virtual void VisualAssemble(Transform JointPosition)
    {
        transform.parent = JointPosition;
        transform.localPosition = Displacement;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public virtual void SetLayer(int Layer)
    {
        foreach (Collider a in GetComponentsInChildren<Collider>())
        {
            a.gameObject.layer = Layer;
        }
    }

    public virtual float GetWeight(bool IncludeGear)
    {
        return Weight;
    }



}
