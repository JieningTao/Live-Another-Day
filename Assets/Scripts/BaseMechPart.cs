﻿using System;
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

            BoostJuiceCapacity,
            BoostJuiceRecovery,
            BoostJuiceRecoveryCooldown,

            //MovingDrag = Drag.x;
            //StoppingDrag = Drag.y;
            //OverSpeedDrag = Drag.z;

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
        MyMech = Mech;
        transform.parent = JointPosition;
        transform.localPosition = Displacement;
        SetLayer(Mech.gameObject.layer);
    }

    public virtual void SetLayer(int Layer)
    {
        foreach (Collider a in GetComponentsInChildren<Collider>())
        {
            a.gameObject.layer = Layer;
        }
    }



}