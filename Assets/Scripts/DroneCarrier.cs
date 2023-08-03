using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCarrier : BaseDrone
{
    [SerializeField]
    List<DroneSubWeapon> SubWeapons = new List<DroneSubWeapon>();
    [SerializeField]
    float RetargetPeriod = 2.0f;
    float RetargetCD;


    protected override void Start()
    {
        base.Start();

        for(int i=0;i<SubWeapons.Count;i++)
        {
            if (!SubWeapons[i].isActiveAndEnabled)
            {
                SubWeapons.RemoveAt(i);
                i--;
            }
        }

        RetargetPeriod = Random.Range(0, RetargetPeriod);
    }

    protected override void Destroied()
    {
        base.Destroied();
        foreach (DroneSubWeapon a in SubWeapons)
        {
            a.GetDestroied();
        }
    }

    private void Update()
    {
        if (RetargetCD > 0)
            RetargetCD -= Time.deltaTime;
        else
        { 
            RetargetWeapons();
            RetargetCD = RetargetPeriod;
        }
    }

    protected virtual void RetargetWeapons()
    {
        if (LockedTargets.Count == 0) // with no targets, skip retargeting phase
            return;

        foreach(DroneSubWeapon a in SubWeapons)
        {
            AssignTarget(a);
        }
    }

    protected virtual void AssignTarget(DroneSubWeapon SW)
    {
        if (!SW.CurrentlyTargeting())
        {
            for (int i = 0; i < LockedTargets.Count; i++)
            {
                int C = Random.Range(0, LockedTargets.Count);

                if (SW.CanTargetPosition(LockedTargets[C].transform.position))
                {
                    SW.RecieveTarget(LockedTargets[C]);
                    break;
                }
            }

            if (!SW.CurrentlyTargeting())
                SW.RecieveTarget(null);
        }
    }







}
