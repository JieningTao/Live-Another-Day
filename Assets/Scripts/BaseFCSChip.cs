using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFCSChip : MonoBehaviour
{
    [SerializeField]
    private int PerLockCount = 1;
    public int GetPerLockCount { get { return PerLockCount; } }
    [SerializeField]
    private float LockTime = 0.8f;
    public float GetLockTime { get { return LockTime; } }
    [SerializeField]
    private int MaxLock = 5;
    public int GetMaxLock { get { return MaxLock; } }
    [Space(10)]
    [SerializeField]
    private float LockRange = 100;
    public float GetLockRange { get { return LockRange; } }
    [SerializeField]
    private float RadarRange = 200;
    public float GetRadarRange { get { return RadarRange; } }
    [Space(10)]
    [SerializeField]
    private float AimAngle = 60;
    public float GetAimAngle { get { return AimAngle; } }

#if UNITY_EDITOR
    public void SetStats(int a,float b,int c, float d, float e, float f)
    {
        PerLockCount = a;
        LockTime = b;
        MaxLock = c;
        LockRange = d;
        RadarRange = e;
        AimAngle = f;

    }
#endif


}
