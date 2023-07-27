using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : IDamageable
{
    [SerializeField]
    protected SphereCollider DetectRadius;
    [SerializeField]
    protected float DetectRange;

    List<EnergySignal> LockedTargets = new List<EnergySignal>();
    protected EnergySignal MTargetSignal;

    [SerializeField]
    protected AIMovement MyMovement;

    //[Serializable]
    //protected class WeaponFireControl
    //{
    //    [SerializeField]
    //    public EnemyGear Weapon;
    //    [SerializeField]
    //    public float FireAngle;
    //    [SerializeField]
    //    public float FireInterval;
    //    [SerializeField]
    //    public float CoolDownInterval;

    //    protected float CurrentIntervalRemaining;
    //    public bool Firing;


    //    public virtual void CheckUpdateFire()
    //    {
    //        if (CurrentIntervalRemaining > 0)
    //        {
    //            CurrentIntervalRemaining -= Time.deltaTime;
    //        }
    //        else
    //        {
    //            if (Firing)
    //                TriggerFire(false);
    //            else if((Weapon as EnemyGearWeapon).GetOffTargetDegree() < FireAngle)
    //                TriggerFire(true);
    //        }
    //    }

    //    protected virtual void TriggerFire(bool Down)
    //    {
    //        Weapon.TriggerGear(Down);
    //        Firing = Down;

    //        if (Down)
    //            CurrentIntervalRemaining = FireInterval;
    //        else
    //            CurrentIntervalRemaining = CoolDownInterval;
    //    }
            

    //}

    protected override void Start()
    {
        base.Start();
        InitializeEnemy();
    }

    public List<EnergySignal> GetTargets(int Amount)
    {
        if (LockedTargets.Count == 0)
            return null;

        List<EnergySignal> Temp = new List<EnergySignal>();

        for (int i = 0; i < Amount; i++)
            Temp.Add(LockedTargets[i % LockedTargets.Count]);

        return Temp;
    }

    public EnergySignal GetMainTarget()
    {
        return MTargetSignal;
    }

    protected virtual void InitializeEnemy()
    {
        DetectRadius.radius = DetectRange;
    }

    protected virtual void AimWeapon(Transform a,Vector3 Dir,Vector3 Limits,float TurnSpeed)
    {
        if (Vector3.Angle(a.forward, Dir) == 0)
            return;

        Vector3 TempDir = Vector3.RotateTowards(a.forward, Dir, TurnSpeed * Time.deltaTime, 0.0f);


        a.rotation = Quaternion.LookRotation(TempDir, transform.up);
        
        //Debug.Log("Ping");

        Vector3 bruh = a.localRotation.eulerAngles; //variable named bruh to commemerate me taking half an hour to realize it wasn't working because turn speed was never changed from initial 0... Fucking idiot.

        bruh.z = 0;

        if (Limits.y==0)
            bruh.y = 0;
        else
        {
            if (bruh.y < 180)
                bruh.y = Mathf.Clamp(bruh.y, -Limits.y, Limits.y);
            else
                bruh.y = Mathf.Clamp(bruh.y, 360 - Limits.y, 360 + Limits.y);
        }

        if (Limits.x ==0)
            bruh.x = 0;
        else
        {
            if (bruh.x < 180)
                bruh.x = Mathf.Clamp(bruh.x, -Limits.x, Limits.x);
            else
                bruh.x = Mathf.Clamp(bruh.x, 360 - Limits.x, 360 + Limits.x);
        }

        a.localRotation = Quaternion.Euler(bruh);


    }




    protected virtual void ReselectMainTarget()
    {
        if (LockedTargets.Count == 0)
            MTargetSignal = null;
        else
            MTargetSignal = LockedTargets[UnityEngine.Random.Range(0, LockedTargets.Count)];
    
    }

    protected void RemoveLock(EnergySignal a)
    {
        LockedTargets.Remove(a);

        if (MTargetSignal == a)
            ReselectMainTarget();
    }

    protected void AddLock(EnergySignal a)
    {
        LockedTargets.Add(a);

        if (MTargetSignal == null)
            ReselectMainTarget();
    }

    protected void OnTriggerEnter(Collider other)
    {
        if ((gameObject.layer == 9 && other.gameObject.layer == 11) || (gameObject.layer == 11 && other.gameObject.layer == 9))
        {
            EnergySignal Temp = other.GetComponentInParent<EnergySignal>();

            if (!Temp)
                return;

            if (!LockedTargets.Contains(Temp))
               AddLock(Temp);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if ((gameObject.layer == 9 && other.gameObject.layer == 11) || (gameObject.layer == 11 && other.gameObject.layer == 9))
        {
            EnergySignal Temp = other.GetComponentInParent<EnergySignal>();

            if (!Temp)
                return;

            if (LockedTargets.Contains(Temp))
                RemoveLock(Temp);
        }
    }




}
