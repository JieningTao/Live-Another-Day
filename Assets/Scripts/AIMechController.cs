using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( BaseMechMain))]
public class AIMechController : MonoBehaviour
{
    [Serializable]
    public class AIControlProfile
    {
        [Header("Movement")]
        public Vector2 BoostDuration = new Vector2(0.5f,2f);
        public Vector2 StrafeDuration = new Vector2(0.5f, 2f);
        [Tooltip("xy for duration of next decision point, z for chance to strafe")]
        public Vector3 StrafeDecision = new Vector3(1, 1.5f, 0.5f);

        [Tooltip("xy for duration, z for chance to Hover")]
        public Vector3 HoverChanceAndDuration = new Vector3(0.5f, 2f,1f);
        [Tooltip("xy for duration of next decision point, z for chance to jump")]
        public Vector3 JumpDecision = new Vector3(1, 1.5f, 0.2f);
        //[Space(20)]
        //[Header("Attack")]

    }

    [SerializeField]
    AIControlProfile MyProfile;
    [SerializeField]
    Transform Target;
    [SerializeField]
    Transform AimReference;
    [SerializeField]
    Vector2 EngageRange;


    BaseMechMain MyMech;
    BaseMechMovement MyMovement;
    BaseMechFCS MyFCS;

    Vector3 PreviousHeading;
    float BoostCD;
    Vector3 CurrentStrafe;
    float StrafeCD;
    float StrafeDecisionCD;


    private void Start()
    {
        MyMech = GetComponent<BaseMechMain>();
        MyFCS = MyMech.GetFCS();
        MyMovement = MyMech.GetMovement();
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotate();
        HandleMovement();
        DecideBoost();
        DecideStrafe();
    }

    void HandleRotate()
    {

        Vector3 AimDir = (Target.position - transform.position).normalized;
        AimDir = AimReference.InverseTransformDirection(AimDir);
        Quaternion ChangeInRot = Quaternion.LookRotation(AimDir, transform.up);
        Vector3 Euler = ChangeInRot.eulerAngles; // the holy grail, rotations from forward of the aimer, negtive for left and up

        if (Euler.x > 180)
            Euler.x = -(360 - Euler.x);
        if (Euler.y > 180)
            Euler.y = -(360 - Euler.y);

        Vector3 TSClamped = Euler;

        TSClamped.x = Mathf.Clamp(TSClamped.x/50, -50 * Time.deltaTime, 50 * Time.deltaTime);
        TSClamped.y = Mathf.Clamp(TSClamped.y/50, -50 * Time.deltaTime, 50 * Time.deltaTime);

        MyMech.Rotate(new Vector3(TSClamped.x, TSClamped.y, 0));
    }

    void HandleMovement()
    {
        if (!Target)
            return;


        Vector3 Temp = CurrentStrafe;

        if (Vector3.Distance(Target.position, transform.position) >= EngageRange.y)
        {
            Temp.z = 1;
            CurrentStrafe.z = 1;
        }
        else if (Vector3.Distance(Target.position, transform.position) <= EngageRange.x)
        {
            Temp.z = -1;
            CurrentStrafe.z = -1;
        }

        Temp.Normalize();

        MyMovement.MovementInput = Temp;
    }

    void DecideBoost()
    {
        if (PreviousHeading != MyMovement.MovementInput)
        {
            if (ShouldBoost())
            {

                MyMovement.BoostControl(true);
                BoostCD = UnityEngine.Random.Range(MyProfile.BoostDuration.x, MyProfile.BoostDuration.y);
            }
        }

        PreviousHeading = MyMovement.MovementInput;

        if (BoostCD > 0)
            BoostCD -= Time.deltaTime;
        else if(MyMovement.Boosting)
            MyMovement.BoostControl(false);

    }

    private bool ShouldBoost()
    {
        if (PreviousHeading == Vector3.zero && MyMovement.MovementInput != Vector3.zero)
            return true;

        if (Vector3.Angle(PreviousHeading, MyMovement.MovementInput) > 45)
            return true;

            return false;
    }

    void DecideStrafe()
    {
        if (StrafeCD > 0)
            StrafeCD -= Time.deltaTime;

        if(StrafeDecisionCD>0)
            StrafeDecisionCD -= Time.deltaTime;

        if (StrafeCD <= 0)
        {
                CurrentStrafe = Vector3.zero;
            StrafeCD = UnityEngine.Random.Range(MyProfile.StrafeDuration.x, MyProfile.StrafeDuration.y);
        }

        if (StrafeDecisionCD <= 0)
        {
            StrafeDecisionCD = UnityEngine.Random.Range(MyProfile.StrafeDecision.x, MyProfile.StrafeDecision.y);
            if (UnityEngine.Random.Range(0f, 1f) < MyProfile.StrafeDecision.z) //decideto strafe or not
            {
                if (UnityEngine.Random.Range(0, 100) > 50)
                    CurrentStrafe.x = 1;
                else
                    CurrentStrafe.x = -1;

                if (UnityEngine.Random.Range(0, 100) > 80)
                {
                    if (UnityEngine.Random.Range(0, 100) > 50)
                        CurrentStrafe.z = 1;
                    else
                        CurrentStrafe.z = -1;
                }
            }
        }
    }
}
