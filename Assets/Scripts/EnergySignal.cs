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

    private Vector3 Speed;
    private Vector3 PreviousPosition;






    private void Update()
    {
        Speed = (transform.position - PreviousPosition) / Time.deltaTime;

        PreviousPosition = transform.position;
    }

    public Vector3 GetSpeed()
    {
        return Speed;
    }

}
