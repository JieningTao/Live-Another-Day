using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class AttributeManager
{

    public enum Attribute
    {
        MoveForce,
        //SpeedLimit,
        JumpForce,

        BoostForce,
        BoostCost,
        ImpulseForce,
        ImpulseCost,
        FloatForce,
        FloatCost,

        AmmoAmount,

        BoostJuiceCapacity,
        BoostJuiceRecovery,
        BoostJuiceRecoveryCooldown,

        RadarRange,
        LockRange,
        MaxLock,
        LockSpeed,


    }

    [Serializable]
    public class BaseMechAttribute
    {
        [SerializeField]
        public Attribute AttributeType;
        [SerializeField]
        public float TributeAmount;

        public List<string> AttStats()
        {
            List<string> Temp = new List<string>();

            Temp.Add(AttributeType.ToString());

            switch (AttributeType)
            {
                case Attribute.BoostCost:
                case Attribute.ImpulseCost:
                case Attribute.FloatCost:
                case Attribute.LockSpeed:
                    if (TributeAmount > 0)
                    {
                        Temp.Add("+" + (TributeAmount * 100) + "%");
                    }
                    else
                    {
                        Temp.Add((TributeAmount * 100) + "%");
                    }

                    break;

                case Attribute.MoveForce:
                case Attribute.JumpForce:
                case Attribute.BoostForce:
                case Attribute.ImpulseForce:
                case Attribute.FloatForce:
                case Attribute.AmmoAmount:
                case Attribute.BoostJuiceCapacity:
                case Attribute.RadarRange:
                case Attribute.LockRange:

                    if (TributeAmount > 0)
                        Temp.Add("+" + TributeAmount);
                    else
                        Temp.Add("" + TributeAmount);

                    break;

                case Attribute.MaxLock:
                    if (TributeAmount > 0)
                        Temp.Add("+" + (int)TributeAmount);
                    else
                        Temp.Add(""+(int)TributeAmount);
                    break;

                case Attribute.BoostJuiceRecovery:
                    if (TributeAmount > 0)
                        Temp.Add("+" + TributeAmount + "/s");
                     else
                        Temp.Add(TributeAmount + "/s");
                    break;

                case Attribute.BoostJuiceRecoveryCooldown:
                    if (TributeAmount > 0)
                        Temp.Add("+" + TributeAmount  + "s");
                    else
                        Temp.Add(TributeAmount + "s");
                    break;
            }

            return Temp;
        }

    }

    [Serializable]
    public class ExtraAttribute
    {
        [SerializeField]
        public string AttributeName;
        [SerializeField]
        public float TributeAmount;
    }

    [Serializable]
    public class AttributeProfile
    {
        public float MoveForce = 0;
        //public float SpeedLimit = 0;
        public float JumpForce = 0;

        public float BoostForce = 0;
        public float BoostCost = 1;
        public float ImpulseForce =0;
        public float ImpulseCost = 1;
        public float FloatForce = 0;
        public float FloatCost = 1;

        public float AmmoAmount = 0;

        public float BoostJuiceCapacity = 0;
        public float BoostJuiceRecovery = 0; 
        public float BoostJuiceRecoveryCooldown = 0;

        public float RadarRange =0;
        public float LockRange =0;
        public float MaxLock =0;
        public float LockSpeed = 1;

        public List<ExtraAttribute> ExtraAttributes = new List<ExtraAttribute>();

        public void ApplyBaseMechAttribute(BaseMechAttribute BMA)
        {
            switch (BMA.AttributeType)
            {
                case Attribute.MoveForce:
                    MoveForce += BMA.TributeAmount;
                    break;
                //case Attribute.SpeedLimit:
                //    SpeedLimit += BMA.TributeAmount;
                //    break;
                case Attribute.JumpForce:
                    JumpForce += BMA.TributeAmount;
                    break;


                case Attribute.BoostForce:
                    BoostForce += BMA.TributeAmount;
                    break;
                case Attribute.BoostCost:
                    BoostCost += BMA.TributeAmount;
                    break;
                case Attribute.ImpulseForce:
                    ImpulseForce += BMA.TributeAmount;
                    break;
                case Attribute.ImpulseCost:
                    ImpulseCost += BMA.TributeAmount;
                    break;
                case Attribute.FloatForce:
                    FloatForce += BMA.TributeAmount;
                    break;
                case Attribute.FloatCost:
                    FloatCost += BMA.TributeAmount;
                    break;


                case Attribute.AmmoAmount:
                    AmmoAmount += BMA.TributeAmount;
                    break;


                case Attribute.BoostJuiceCapacity:
                    BoostJuiceCapacity += BMA.TributeAmount;
                    break;
                case Attribute.BoostJuiceRecovery:
                    BoostJuiceRecovery += BMA.TributeAmount;
                    break;
                case Attribute.BoostJuiceRecoveryCooldown:
                    BoostJuiceRecoveryCooldown += BMA.TributeAmount;
                    break;


                case Attribute.RadarRange:
                    RadarRange += BMA.TributeAmount;
                    break;
                case Attribute.LockRange:
                    LockRange += BMA.TributeAmount;
                    break;
                case Attribute.MaxLock:
                    MaxLock += BMA.TributeAmount;
                    break;
                case Attribute.LockSpeed:
                    LockSpeed += BMA.TributeAmount;
                    break;
            }
        }

        public void ApplyExtraAttribute(ExtraAttribute EA)
        {
            for (int i = 0; i < ExtraAttributes.Count; i++)
            {
                if (ExtraAttributes[i].AttributeName == EA.AttributeName)
                {
                    ExtraAttributes[i].TributeAmount += EA.TributeAmount;
                    return;
                }
            }
            ExtraAttributes.Add(EA);
        }

        public ExtraAttribute FetchAttribute(string ExtraAttributeName)
        {
            for (int i = 0; i < ExtraAttributes.Count; i++)
            {
                if (ExtraAttributes[i].AttributeName == ExtraAttributeName)
                    return ExtraAttributes[i];
            }
            return null;
        }
    }

}
