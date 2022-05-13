using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySignal : MonoBehaviour
{

    public string SignalName;
    public EnergySignalType MyType;


    public enum EnergySignalType
    {
        LowEnergy,
        Mech,
        Missile,
        HES,//high energy signiture

        Other,



    }

}
