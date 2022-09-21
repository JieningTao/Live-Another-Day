using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTripod : BaseEnemy
{
    [Space(15)]
    [SerializeField]
    Transform AICore;
    [SerializeField]
    Transform AIBody;
    [SerializeField]
    float AICoreLookSpeed;
    [SerializeField]
    private float RandomLookAngle = 30;
    [SerializeField]
    private Vector2 LookInterval = new Vector2(3,5);

    [SerializeField]
    EnemyGear LeftWeapon;
    //[SerializeField]
    //WeaponFireControl LeftFireControl;
    //[SerializeField]
    //EnemyGear RightWeapon;
    //WeaponFireControl RightFireControl;
    [SerializeField]
    EnemyGear BackWeapon;
    [SerializeField]
    float WeaponTurnSpeed = 1;
    [SerializeField]
    float SelfTurnSpeed = 0.8f;
    [SerializeField]
    Vector3 WeaponAimLimits = new Vector3(10,80,0);


    Vector3 RandomLookDirection;
    float LookCooldown;


    //[Serializable]
    //public class TripodWeapon
    //{
    //    //[SerializeField]
    //    //some sort of direct control to the gear
    //    [SerializeField]
    //    bool VerticallyAimed;
    //    [SerializeField]
    //    bool HorizontallyAimed;
    //    [SerializeField]
    //    Vector2 Range;
    //    [SerializeField]
    //    float Cooldown;
    //    [SerializeField]
    //    float BurstDuretion;
    //    [SerializeField]
    //    float MaxAngleDeviation;
    //}



    protected override void Start()
    {
        base.Start();
        DecideNextLook();
    }

    private void Update()
    {
        LookUpdate();
        AimWeapons();
        AimSelf();
        CheckWeaponFire();

    }

    private void CheckWeaponFire()
    {
        //if (LeftWeapon)
        //    LeftFireControl.CheckUpdateFire();
        //if (RightWeapon)
        //    RightFireControl.CheckUpdateFire();
    }

    protected override void InitializeEnemy()
    {
        base.InitializeEnemy();
        InitializeFireControl();
    }

    protected void InitializeFireControl()
    {
        //if (LeftWeapon)
        //{
        //    LeftFireControl.Weapon = LeftWeapon;
        //    LeftWeapon.AssignController(this);
        //}
        //if (RightWeapon)
        //{
        //    RightFireControl.Weapon = RightWeapon;
        //    RightWeapon.AssignController(this);
        //}
    }

    protected void LookUpdate()
    {
        if (MTargetSignal)
            UpdateAICoreLook(1);
        else
            UpdateAICoreLook(0.5f);

        LookCooldown -= Time.deltaTime;

        if (LookCooldown < 0)
            DecideNextLook();
    }

    private void AimSelf()
    {
        if (MTargetSignal)
        {
            AimWeapon(AIBody, MTargetSignal.transform.position - LeftWeapon.transform.position,new Vector3(0,360,0), SelfTurnSpeed);
        }
    }

    private void AimWeapons()
    {
        //if (MTargetSignal)
        //{
        //    if (LeftWeapon && LeftWeapon.Aimed)
        //        AimWeapon(LeftWeapon.transform, MTargetSignal.transform.position - LeftWeapon.transform.position, WeaponAimLimits, WeaponTurnSpeed);
        //    if (RightWeapon && RightWeapon.Aimed)
        //        AimWeapon(RightWeapon.transform, MTargetSignal.transform.position - RightWeapon.transform.position, WeaponAimLimits, WeaponTurnSpeed);
        //}
        //else
        //{
        //    if (LeftWeapon && LeftWeapon.Aimed)
        //        AimWeapon(LeftWeapon.transform, LeftWeapon.transform.forward, WeaponAimLimits, WeaponTurnSpeed);
        //    if (RightWeapon && RightWeapon.Aimed)
        //        AimWeapon(RightWeapon.transform, RightWeapon.transform.forward, WeaponAimLimits, WeaponTurnSpeed);
        //}


    }

    protected override void ReselectMainTarget()
    {
        base.ReselectMainTarget();
        DecideNextLook();
    }

    private void UpdateAICoreLook(float SpeedMultiplier)
    {
        Vector3 newDir;
        if (RandomLookDirection != Vector3.zero)
        {
            newDir = Vector3.RotateTowards(AICore.transform.forward, RandomLookDirection, AICoreLookSpeed * Time.deltaTime * SpeedMultiplier, 0.0f);
        }
        else if (MTargetSignal != null)
        {
            newDir = Vector3.RotateTowards(AICore.transform.forward, MTargetSignal.transform.position - AICore.transform.position, AICoreLookSpeed * Time.deltaTime * SpeedMultiplier, 0.0f);
        }
        else
        {
            newDir = Vector3.RotateTowards(AICore.transform.forward, AICore.transform.forward, AICoreLookSpeed * Time.deltaTime * SpeedMultiplier, 0.0f);
        }

        AICore.transform.rotation = Quaternion.LookRotation(newDir, transform.up);

        Debug.DrawRay(AICore.transform.position,AICore.transform.forward, Color.red);
        Debug.DrawRay(AICore.transform.position, transform.forward, Color.blue);
    }

    private void DecideNextLook()
    {
        if (!MTargetSignal)
        {
            if (UnityEngine.Random.Range(0, 100) < 33)
            {
                RandomLookDirection = Vector3.forward;
            }
            else
                RandomLookDirection = Quaternion.AngleAxis(UnityEngine.Random.Range(-RandomLookAngle, RandomLookAngle), new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1))) * transform.forward;
        }
        else
        {
            if (UnityEngine.Random.Range(0, 100) < 10)
            {
                RandomLookDirection = Quaternion.AngleAxis(UnityEngine.Random.Range(-RandomLookAngle, RandomLookAngle), new Vector3(UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1), UnityEngine.Random.Range(-1, 1))) * transform.forward;
            }
            else
                RandomLookDirection = Vector3.zero;

        }
        LookCooldown = UnityEngine.Random.Range(LookInterval.x, LookInterval.y);
    }

}
