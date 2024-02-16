using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechStatsDisplayMaster : MonoBehaviour
{

    [SerializeField]
    private MechCompareStat Health;
    [SerializeField]
    private MechCompareStat Weight;

    [Space(10)]

    [SerializeField]
    private MechCompareStat EnergyCapacity;
    [SerializeField]
    private MechCompareStat ReactorGeneration;
    [SerializeField]
    private MechCompareStat PassiveDrain;
    [SerializeField]
    private MechCompareStat CombatOutput;

    [Space(10)]

    [SerializeField]
    private MechCompareStat RadarRange;
    [SerializeField]
    private MechCompareStat LockRange;
    [SerializeField]
    private MechCompareStat LockAngle;

    [Space(10)]

    [SerializeField]
    private MechCompareStat MoveForce;
    [SerializeField]
    private MechCompareStat JumpForce;
    [SerializeField]
    private MechCompareStat BoostForce;







    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
