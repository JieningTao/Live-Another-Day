using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLockEnergySignal : EnergySignal
{
    [SerializeField]
    public MLSignalType MySignalType;

    public enum MLSignalType
    {
        LowEnergy,
        MidEnergy,
        HighEnergy,
        Mech,
        Special,
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
