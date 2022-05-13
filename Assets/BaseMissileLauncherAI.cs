using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMissileLauncherAI : MonoBehaviour
{


    [SerializeField]
    BaseMissileLauncher MyWeapon;

    [SerializeField]
    int BurstAmount =3;
    [SerializeField]
    float BurstInterval = 3;
    [SerializeField]
    bool FocusMain = false;

    protected EnergySignal MainTarget;
    List<EnergySignal> TargetsWithinRange = new List<EnergySignal>();

    float IntervalCooldown;

    private void Update()
    {
        TargetCheck();

        if (IntervalCooldown > 0)
            IntervalCooldown -= Time.deltaTime;
        else
        {
            Fire();
            IntervalCooldown = BurstInterval;
        }
    }

    private void Fire()
    {
        List<EnergySignal> Temp = new List<EnergySignal>();
        for (int i = 0; i < BurstAmount; i++)
        {
            if (FocusMain)
                Temp.Add(MainTarget);
            else
                Temp.Add(TargetsWithinRange[Random.Range(0, TargetsWithinRange.Count)]);
        }
        MyWeapon.FireVolly(Temp);
    }

    private void TargetCheck()
    {
        for (int i = 0; i < TargetsWithinRange.Count; i++)
        {
            if (TargetsWithinRange[i] == null || !TargetsWithinRange[i].enabled)
            {
                TargetsWithinRange.RemoveAt(i);
                i--;
            }
        }

        if (MainTarget == null || !TargetsWithinRange.Contains(MainTarget))
        {
            if (TargetsWithinRange.Count > 0)
                MainTarget = TargetsWithinRange[Random.Range(0, TargetsWithinRange.Count)];
            else
                MainTarget = null;
        }
    }




    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
            return;

        if ((gameObject.layer == 9 && other.gameObject.layer == 11) || (gameObject.layer == 11 && other.gameObject.layer == 9))
        {
            EnergySignal Temp = other.GetComponentInParent<EnergySignal>();

            if (!Temp)
                return;

            if (!TargetsWithinRange.Contains(Temp))
            {
                TargetsWithinRange.Add(Temp);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.isTrigger)
            return;

        if ((gameObject.layer == 9 && other.gameObject.layer == 11) || (gameObject.layer == 11 && other.gameObject.layer == 9))
        {
            EnergySignal Temp = other.GetComponentInParent<EnergySignal>();

            if (!Temp)
                return;

            if (TargetsWithinRange.Contains(Temp))
                TargetsWithinRange.Remove(Temp);
        }
    }
}
