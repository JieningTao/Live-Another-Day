using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTurretAI : MonoBehaviour
{
    [SerializeField]
    BaseTurret MyTurret;
    [SerializeField]
    BaseShoot MyWeapon;
    [SerializeField]
    float MaxAllowedAngleDeviation;

    [SerializeField]
    float BurstDuration;
    [SerializeField]
    float BurstIntermission;


    //public TurretAIState MyAIState;
    //public enum TurretAIState
    //{
    //    Rest,
    //    FAW, //Fire At Will (Poor Will)
    //    Hold,
    //}

    public TurretState MyCurrentState;
    public enum TurretState
    {
        Resting,
        Tracking,
        Firing,
    }

    protected float StateTimer;
    List<EnergySignal> TargetsWithinRange = new List<EnergySignal>();



    private void Update()
    {

        switch (MyCurrentState)
        {
            case TurretState.Resting:
                break;

            case TurretState.Tracking:
                CheckForNullTarget();
                if (StateTimer > 0)
                    StateTimer -= Time.deltaTime;
                else if(CheckToFire())    
                        SetTurretState(TurretState.Firing);
                break;

            case TurretState.Firing:
                CheckForNullTarget();
                if (StateTimer > 0)
                    StateTimer -= Time.deltaTime;
                else
                    SetTurretState(TurretState.Tracking);
                break;
        }


    }

    private void SetTurretState(TurretState a)
    {
        MyCurrentState = a;

        if (a == TurretState.Resting)
        {
            MyTurret.TurnToRest();
            MyWeapon.Trigger(false);
        }
        else if (a == TurretState.Tracking)
        {
            MyWeapon.Trigger(false);
            StateTimer = BurstIntermission;
        }
        else if (a == TurretState.Firing)
        {
            MyWeapon.Trigger(true);
            StateTimer = BurstDuration;
        }

    }


    //public void SetAIState(TurretAIState a)
    //{
    //    MyAIState = a;
    //}


    private bool CheckToFire()
    {
        if (MyTurret.GetTargetAngleDeviation() <= MaxAllowedAngleDeviation)
            return true;
        else
            return false;
    }

    private void CheckForNullTarget()
    {
        for (int i = 0; i < TargetsWithinRange.Count; i++)
        {
            if (TargetsWithinRange[i] == null)
            {
                TargetsWithinRange.RemoveAt(i);
                i--;
            }
        }


        if (MyTurret.IsResting() || MyTurret.Target == null||MyTurret.TargetSignal == null || !MyTurret.TargetSignal.enabled)
        {
            if (TargetsWithinRange.Count > 0)
                MyTurret.Target = TargetsWithinRange[Random.Range(0, TargetsWithinRange.Count)].gameObject;
            else
                SetTurretState(TurretState.Resting);
        }
        else
            return;
    }

    private void AssignNewTarget(EnergySignal a)
    {
        MyTurret.TargetSignal = a;
        MyTurret.Target = a.gameObject;
        SetTurretState(TurretState.Tracking);
    }

    private void AssignRandomTarget()
    {
        AssignNewTarget(TargetsWithinRange[Random.Range(0, TargetsWithinRange.Count)]);
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.isTrigger)
            return;

        if ((gameObject.layer == 9 && other.gameObject.layer == 11)||(gameObject.layer == 11 && other.gameObject.layer == 9))
        {
            EnergySignal Temp = other.GetComponentInParent<EnergySignal>();

            if (!Temp)
                return;

            if (MyCurrentState == TurretState.Resting)
                SetTurretState(TurretState.Tracking);

            if (!TargetsWithinRange.Contains(Temp))
            {



                TargetsWithinRange.Add(Temp);

                if (MyTurret.IsResting() || MyTurret.Target == null)
                    AssignNewTarget(Temp);
                else if (Random.Range(0, 100) < 10)
                    AssignRandomTarget();

                //Debug.Log(Temp.gameObject.name);
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

            if (TargetsWithinRange.Count == 0)
                MyTurret.TurnToRest();

        }
    }
}
