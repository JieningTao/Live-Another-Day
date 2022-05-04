using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechFCS : MonoBehaviour
{
    [SerializeField]
    private Transform CameraAnchor;
    [SerializeField]
    private Transform RightHand;
    [SerializeField]
    private Transform Lefthand;
    [SerializeField]
    private float TargetSpeed;
    [SerializeField]
    private float MaxAimingAngle = 60;

    [SerializeField]
    private BaseMainSlotEquipment CurrentPrimary;

    [SerializeField]
    private BaseMainSlotEquipment CurrentSecondary;

    [SerializeField]
    public List<EnergySignal> TargetsWithinRange = new List<EnergySignal>();

    [SerializeField]
    private bool PlayerFCS = false;
    public static event Action<String,EnergySignal> LockChanges;
    public static event Action<bool, BaseMainSlotEquipment> WeaponChanges;

    [SerializeField]
    public float RadarRange  = 200;
    [SerializeField]
    public float LockRange  = 150;

    [SerializeField] // SFT
    public EnergySignal MainTarget; //public for testing
    private Vector3 TargetPosition;

    private void Start()
    {
        if (WeaponChanges != null)
        {
            WeaponChanges.Invoke(true, CurrentPrimary);
            WeaponChanges.Invoke(false, CurrentSecondary);
        }
    }

    private void Update()
    {
        CheckLockValidity();
        FindMainLock();

        Targetweapons(Lefthand);
        Targetweapons(RightHand);
    }

    private void CheckLockValidity()
    {
        for (int i = 0; i < TargetsWithinRange.Count; i++)
        {
            if (TargetsWithinRange[i] == null || !TargetsWithinRange[i].enabled)
            {
                RemoveLock(TargetsWithinRange[i]);
                i--;
            }
        }
    }

    public void FirePrimary1(bool _Fire)
    {
        CurrentPrimary.PrimaryFire(_Fire);
    }

    public void FirePrimary2(bool _Fire)
    {
        //Debug.Log("1");
        CurrentPrimary.SecondaryFire(_Fire);
    }

    public void FireSecondary1(bool _Fire)
    {
        CurrentSecondary.PrimaryFire(_Fire);
    }

    public void FireSecondary2(bool _Fire)
    {
        CurrentSecondary.SecondaryFire(_Fire);
    }

    public BaseMainSlotEquipment SwitchPrimaryEquipment(BaseMainSlotEquipment NewEquipment)
    {
        BaseMainSlotEquipment Temp = CurrentPrimary;

        CurrentPrimary = NewEquipment;

        return Temp;
    }

    public BaseMainSlotEquipment SwitchSecondaryEquipment(BaseMainSlotEquipment NewEquipment)
    {
        BaseMainSlotEquipment Temp = CurrentSecondary;

        CurrentSecondary = NewEquipment;

        return Temp;
    }

    private void Targetweapons(Transform ThingToAim)
    {
        Vector3 AimDir;

        if (MainTarget)
        {
            TargetPosition = MainTarget.transform.position;
            AimDir = Vector3.RotateTowards(ThingToAim.forward, TargetPosition - ThingToAim.transform.position, TargetSpeed * Time.deltaTime, 0.0f);
        }
        else
        {
            AimDir = Vector3.RotateTowards(ThingToAim.forward, CameraAnchor.forward, TargetSpeed * Time.deltaTime, 0.0f);
        }


        ThingToAim.rotation = Quaternion.LookRotation(AimDir, this.transform.up);

    }


    private void FindMainLock()
    {
        EnergySignal OldTarget = MainTarget;




        float MTAngle;

        if (MainTarget == null )
            MTAngle = 0;
        else
        {
            MTAngle = Vector3.Angle(CameraAnchor.forward,  MainTarget.transform.position- transform.position);
            if (MTAngle > MaxAimingAngle || !MainTarget.enabled|| Vector3.Distance(MainTarget.transform.position, transform.position) > LockRange)
            {
                MTAngle = 0;
                MainTarget = null;
            }
        }

        for (int i = 0; i < TargetsWithinRange.Count; i++)
        {
            float NTAngle = Vector3.Angle(CameraAnchor.forward,  TargetsWithinRange[i].transform.position - transform.position );

            if (NTAngle < MaxAimingAngle)
            {
                if ( Vector3.Distance( TargetsWithinRange[i].transform.position,transform.position)<LockRange && (MainTarget == null || NTAngle < MTAngle))
                {
                    //Debug.Log("FCS Targting " + TargetsWithinRange[i].name);
                    MainTarget = TargetsWithinRange[i];
                    MTAngle = NTAngle;
                }
            }
        }

        if (OldTarget != MainTarget)
        {
            if (PlayerFCS && LockChanges != null)
                LockChanges.Invoke("Target", MainTarget);
        }
    }


    private void AddLock(EnergySignal a)
    {
        TargetsWithinRange.Add(a);
        if (PlayerFCS && LockChanges != null)
            LockChanges.Invoke("Add", a);
    }

    private void RemoveLock(EnergySignal a)
    {
        TargetsWithinRange.Remove(a);
        if (MainTarget == a)
            MainTarget = null;
        if (PlayerFCS && LockChanges != null)
            LockChanges.Invoke("Remove", a);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;


        if ((gameObject.layer == 9 && other.gameObject.layer == 11) || (gameObject.layer == 11 && other.gameObject.layer == 9))
        {
            EnergySignal Temp = other.GetComponentInParent<EnergySignal>();

            if (!Temp||!Temp.enabled)
                return;

            if (!TargetsWithinRange.Contains(Temp))
            {
                AddLock(Temp);
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
                RemoveLock(Temp);
        }
    }
}
