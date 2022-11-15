using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePowerSystem :BaseEnergySource
{

    #region Loadoutpart request info stuff

    public virtual string GetCapacity
    { get { return MaxEnergy+""; } }

    public virtual string GetRegen
    { get { return NaturalEnergyRegen+""; } }

    #endregion
}
