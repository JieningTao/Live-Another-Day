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
    private SphereCollider RadarCollider;
    [SerializeField]
    public float RadarRange  = 200;
    [SerializeField]
    public float LockRange  = 150;

    [SerializeField]
    private float LockTime;

    [SerializeField] // SFT
    public EnergySignal MainTarget; //public for testing
    private Vector3 TargetPosition;
    private List<EnergySignal> CurrentListOfLocked = new List<EnergySignal>();
    private float LockCooldown;
    private bool CurrentlyLocking;
    private int LockRequested;

    private bool FocusMode;
    private EnergySignal FocusTarget;

    private void Start()
    {
        if (WeaponChanges != null)
        {
            WeaponChanges.Invoke(true, CurrentPrimary);
            WeaponChanges.Invoke(false, CurrentSecondary);
        }

        RadarCollider.radius = RadarRange;
        RadarCollider.isTrigger = true;
    }

    private void Update()
    {
        //try to target weapon first to avoid not aiming correctly at targets
        Targetweapons(Lefthand);
        Targetweapons(RightHand);

        CheckTargetValidity();
        FindMainLock();

        UpdateLockedList();

    }




    private void CheckTargetValidity()
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

    private void UpdateLockedList()
    {
        if (CurrentlyLocking&&CurrentListOfLocked.Count<LockRequested)
        {
            LockCooldown -= Time.deltaTime;
            if (LockCooldown <= 0)
            {
                LockClosestUnlocked(1);
            }
        }
    }

    public void RequestLocks(int Amount)
    {
        CurrentlyLocking = true;
        LockRequested = Amount;
        LockCooldown = LockTime;
    }

    private void LockClosestUnlocked(int Amount)
    {
        for (int i = 0; i < CurrentListOfLocked.Count; i++)
        {
            if (CurrentListOfLocked[i] == null || !TargetsWithinRange.Contains(CurrentListOfLocked[i]))
            {
                CurrentListOfLocked.RemoveAt(i);
                i--;
            }
        }

        if (MainTarget != null && !CurrentListOfLocked.Contains(MainTarget))
        {
            LockChanges.Invoke("Lock", MainTarget);
            CurrentListOfLocked.Add(MainTarget);
            LockCooldown = LockTime;
        }
        else
        {

            List<EnergySignal> Temp = AngleSortedTargetList();

            for (int i = 0; i < Temp.Count; i++)
            {
                if (!CurrentListOfLocked.Contains(Temp[i]))
                {
                    CurrentListOfLocked.Add(Temp[i]);
                    LockChanges.Invoke("Lock", Temp[i]);

                    Amount--;
                    if (Amount <= 0)
                    {
                        LockCooldown = LockTime;
                        break;
                    }
                }
            }

        }

    }

    private List<EnergySignal> AngleSortedTargetList()
    {
        List<EnergySignal> Temp = new List<EnergySignal>();

        for (int i = 0; i < TargetsWithinRange.Count; i++)
        {
            if (i == 0)
                Temp.Add(TargetsWithinRange[i]);
            else
            {
                float NewAngle = Vector3.Angle(CameraAnchor.forward, TargetsWithinRange[i].transform.position - transform.position);

                for (int j = 0; j < Temp.Count; j++)
                {

                    if (NewAngle < Vector3.Angle(CameraAnchor.forward, Temp[j].transform.position - transform.position))
                    {
                        Temp.Insert(j, TargetsWithinRange[i]);
                        break;
                    }
                    else if (j == Temp.Count - 1)
                    {
                        Temp.Insert(j, TargetsWithinRange[i]);
                        break;
                    }
                }
            }

        }

        return Temp;
    }

    public List<EnergySignal> GetLockedList()
    {
        List<EnergySignal> Temp = new List<EnergySignal>();
        Temp.AddRange(CurrentListOfLocked);

        CurrentListOfLocked.Clear();

        CurrentlyLocking = false;
        LockCooldown = LockTime;
        LockChanges.Invoke("UnlockAll", null);
        return Temp;
    }

    private void FindMainLock()
    {
        EnergySignal OldTarget = MainTarget;

        if (FocusMode)
        {
            if (Vector3.Angle(CameraAnchor.forward, FocusTarget.transform.position - transform.position) < MaxAimingAngle && Vector3.Distance(MainTarget.transform.position, transform.position) < LockRange)
            {
                //in focus mode, always target focus target if possible
                MainTarget = FocusTarget;

                if (OldTarget != MainTarget)
                {
                    if (PlayerFCS && LockChanges != null)
                        LockChanges.Invoke("Target", MainTarget);
                }

                return;
            }
        }

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

    public void ToggleFocusMode()
    {
        ToggleFocusMode(!FocusMode);
    }

    public void ToggleFocusMode(bool a)
    {
        FocusMode = a;
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
