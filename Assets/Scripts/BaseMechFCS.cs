using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechFCS : MonoBehaviour
{

    private BaseMechPartArm LeftArm;
    private BaseMechPartArm RightArm;

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

    private bool PlayerFCS = false;
    public static event Action<String,EnergySignal> LockChanges;
    public static event Action<bool, BaseMainSlotEquipment> WeaponChanges;
    public static event Action<int,string, BaseEXGear> EXGearChanges;

    [SerializeField]
    private SphereCollider RadarCollider;
    [SerializeField]
    public float RadarRange  = 200;
    [SerializeField]
    public float LockRange  = 150;

    [Space(10)]

    [SerializeField]
    protected BaseEXGear LeftLegEXG;
    [SerializeField]
    protected BaseEXGear LeftArmEXG;
    [SerializeField]
    protected BaseEXGear LeftShoulderEXG;

    [SerializeField]
    protected BaseEXGear RightShoulderEXG;
    [SerializeField]
    protected BaseEXGear RightArmEXG;
    [SerializeField]
    protected BaseEXGear RightLegEXG;

    protected BaseEXGear[] EquipedEXGear = new BaseEXGear[8];

    [Space(10)]


    protected int SelectedEXSlot;

    [SerializeField]
    private float LockTime;

    private Transform CameraAnchor;
    protected EnergySignal MainTarget; //public for testing
    private Vector3 TargetPosition;
    private List<EnergySignal> CurrentListOfLocked = new List<EnergySignal>();
    private float LockCooldown;
    private bool CurrentlyLocking;
    private int LockRequested;
    private BaseMechMain MyBMM;
    private bool FocusMode;
    private EnergySignal FocusTarget;

    //private void Start()
    //{
    //    InitializeFCS(GetComponent<BaseMechMain>());
    //}

    public void InitializeFCS(BaseMechMain BMM,bool Player, BaseMechPartArm _LeftArm, BaseMechPartArm _RightArm)
    {
        SpawnItems();

        MyBMM = BMM;
        LeftArm = _LeftArm;
        RightArm = _RightArm;
        PlayerFCS = Player;

        Equip(CurrentPrimary, true);
        Equip(CurrentSecondary, false);

        RadarCollider.radius = RadarRange;
        RadarCollider.isTrigger = true;

        InitializeEXGear();

        CameraAnchor = BMM.CameraAnchor;
    }

    private void GetInnateEXG()
    {


    }

    private void SpawnItems()
    {
        if(CurrentPrimary)
        CurrentPrimary = Instantiate(CurrentPrimary.gameObject,transform).GetComponent<BaseMainSlotEquipment>();
        if(CurrentSecondary)
        CurrentSecondary = Instantiate(CurrentSecondary.gameObject, transform).GetComponent<BaseMainSlotEquipment>();

        if(LeftLegEXG)
        LeftLegEXG = Instantiate(LeftLegEXG.gameObject, transform).GetComponent<BaseEXGear>();
        if(LeftArmEXG != null)
        LeftArmEXG = Instantiate(LeftArmEXG.gameObject, transform).GetComponent<BaseEXGear>(); 
        if(LeftShoulderEXG)
        LeftShoulderEXG = Instantiate(LeftShoulderEXG.gameObject, transform).GetComponent<BaseEXGear>(); 
        //ChestEXG = Instantiate(LeftLegEXG.gameObject, transform).GetComponent<BaseEXGear>();
        //BackPackEXG;
        if(RightShoulderEXG)
        RightShoulderEXG = Instantiate(RightShoulderEXG.gameObject, transform).GetComponent<BaseEXGear>();
        if (RightArmEXG != null)
        RightArmEXG = Instantiate(RightArmEXG.gameObject, transform).GetComponent<BaseEXGear>();
        if(RightLegEXG)
        RightLegEXG = Instantiate(RightLegEXG.gameObject, transform).GetComponent<BaseEXGear>();
    }

    public void InitStats(float _RadarRange, float _LockRange)
    {
        RadarRange = _RadarRange;
        LockRange = _LockRange;

        RadarCollider.radius = RadarRange;
        RadarCollider.isTrigger = true;


    }

    private void InitializeEXGear()
    {
        EquipedEXGear = new BaseEXGear[8];

        EquipedEXGear[0] = LeftLegEXG;
        EquipedEXGear[1] = LeftArmEXG;
        EquipedEXGear[2] = LeftShoulderEXG;
        //EquipedEXGear[3] = ChestEXG;
        //EquipedEXGear[4] = BackPackEXG;
        EquipedEXGear[5] = RightShoulderEXG;
        EquipedEXGear[6] = RightArmEXG;
        EquipedEXGear[7] = RightLegEXG;



        MyBMM.EXGInstall(ref EquipedEXGear); //requires mech assembly to be set up

        Debug.Log(PlayerFCS);

        for (int i = 0; i < EquipedEXGear.Length; i++)
        {
            if (PlayerFCS && EXGearChanges != null)
                EXGearChanges.Invoke(i + 1, "New", EquipedEXGear[i]);

            //if (EquipedEXGear[i] != null)
            //{
            //    if (i < 3)
            //        EquipedEXGear[i].InitializeGear(MyBMM,null, false);
            //    else
            //        EquipedEXGear[i].InitializeGear(MyBMM,null, true);
            //}
        }

        SelectedEXSlot = 0;

        if (EquipedEXGear[0] == null)
            SwitchEXGear(true);
        else
            SelectSlot(0);
    }

    //public void InitializeEXGear()
    //{
    //    for (int i = 0; i < 8; i++)
    //    {
    //        if (EquipedEXGear[i] != null)
    //        {

    //            EquipedEXGear[i].InitializeGear(MyBMM, false);
    //        }
    //    }
    //}

    private void Update()
    {
        //try to target weapon first to avoid not aiming correctly at targets
        TargetArms();

        CheckTargetValidity();
        FindMainLock();

        UpdateLockedList();

    }

    private void TargetArms()
    {
        if (MainTarget)
        {
            LeftArm.TargetArm(MainTarget);
            RightArm.TargetArm(MainTarget);
        }
        else
        {
            LeftArm.TargetArmEmpty();
            RightArm.TargetArmEmpty();
        }
    }

    public EnergySignal GetMainTarget()
    {
        return MainTarget;
    }

    public EnergySignal GetFocusTarget()
    {
        return FocusTarget;
    }

    public Vector3 GetLookDirection()
    {
        return CameraAnchor.forward;
    }

    protected void Equip(BaseMainSlotEquipment Equipment,bool OnRightHand)
    {
        if (Equipment == null)
        {
            if (WeaponChanges != null)
                WeaponChanges.Invoke(OnRightHand, Equipment);

            if (OnRightHand)
                CurrentPrimary = null;
            else
                CurrentSecondary = null;

            return;
        }

        if (OnRightHand)
        {
            CurrentPrimary = Equipment;

            RightArm.EquipEquipment(Equipment);

            Equipment.Equip(true, MyBMM);
        }
        else
        {
            CurrentSecondary = Equipment;

            LeftArm.EquipEquipment(Equipment);

            Equipment.Equip(true, MyBMM);
        }

        Equipment.transform.localPosition = Vector3.zero;
        Equipment.transform.localRotation = Quaternion.Euler(Vector3.zero);

        


        if (WeaponChanges != null)
            WeaponChanges.Invoke(OnRightHand, Equipment);
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

    public int GetArmHoldPosition(bool Right)
    {
        if (Right)
        {
            if (!CurrentPrimary)
                return 0;
            else
                return (int)CurrentPrimary.HoldStyle;
        }
        else
        {
            if (!CurrentSecondary)
                return 0;
            else
            return (int)CurrentSecondary.HoldStyle;
        }
    }


    public void FirePrimary1(bool _Fire)
    {
        if (CurrentPrimary)
            CurrentPrimary.PrimaryFire(_Fire);
    }

    public void FirePrimary2(bool _Fire)
    {
        //Debug.Log("1");
        if (CurrentPrimary)
            CurrentPrimary.SecondaryFire(_Fire);
    }

    public void FireSecondary1(bool _Fire)
    {
        if (CurrentSecondary)
            CurrentSecondary.PrimaryFire(_Fire);
    }

    public void FireSecondary2(bool _Fire)
    {
        if (CurrentSecondary)
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

    private void Targetweapons(Transform ThingToAim,bool AtTarget)
    {
        Vector3 AimDir;
        
        if (MainTarget && AtTarget)
        {
            AimDir = Vector3.RotateTowards(ThingToAim.forward, MainTarget.transform.position - ThingToAim.transform.position, TargetSpeed * Time.deltaTime, 0.0f);
        }
        else
        {
            AimDir = Vector3.RotateTowards(ThingToAim.forward, CameraAnchor.forward, TargetSpeed * Time.deltaTime, 0.0f);
        }


        ThingToAim.rotation = Quaternion.LookRotation(AimDir, this.transform.up);

    }
    #region LockedList related
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
        if (Amount == 0)
        {
            CurrentlyLocking = false;
            LockRequested = Amount;
            LockCooldown = 0;
            CurrentListOfLocked.Clear();
        }

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

    public int GetLockedAmount()
    {
        return CurrentListOfLocked.Count;
    }
#endregion
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
        if(a.MyType == EnergySignal.EnergySignalType.LowEnergy || a.MyType == EnergySignal.EnergySignalType.Mech)
        TargetsWithinRange.Add(a);


        if (PlayerFCS && LockChanges != null)
            LockChanges.Invoke("Add", a);
    }



    public void ToggleFocusMode()
    {
        ToggleFocusMode(!FocusMode);
    }

    public void ToggleFocusMode(bool a)
    {
        FocusMode = a;
    }

    #region EXGear related

    public void TriggerEXGear(bool TriggerDown)
    {
        if (EquipedEXGear[SelectedEXSlot] != null)
            EquipedEXGear[SelectedEXSlot].TriggerGear(TriggerDown);
        
    }

    public void SwitchEXGear(bool Next)
    {
        int Temp = SwitchSlot(Next);

        if(Temp!=SelectedEXSlot)
        SelectSlot(Temp);
    }

    private void SelectSlot(int SlotNum)
    {
        if(EquipedEXGear[SelectedEXSlot]!=null)
        EquipedEXGear[SelectedEXSlot].Equip(false);

        SelectedEXSlot = SlotNum;

        EquipedEXGear[SelectedEXSlot].Equip(true);

        if (PlayerFCS && EXGearChanges != null)
            EXGearChanges.Invoke(SelectedEXSlot + 1, "Select", null);
    }

    private int SwitchSlot(bool Next)
    {
        int step;

        if (Next)
            step = 1;
        else
            step = -1;

        for (int i = SelectedEXSlot+step; i < EquipedEXGear.Length && i >= 0; i += step)
        {
            if (EquipedEXGear[i] != null)
                return i;
        }

        return SelectedEXSlot;
    }

    #endregion


    private void RemoveLock(EnergySignal a)
    {
        if (a.MyType == EnergySignal.EnergySignalType.LowEnergy || a.MyType == EnergySignal.EnergySignalType.Mech)
            TargetsWithinRange.Remove(a);

        if (MainTarget == a)
            MainTarget = null;
        if (PlayerFCS && LockChanges != null)
            LockChanges.Invoke("Remove", a);
    }

    private void NewLock(EnergySignal a)
    {
        if ((gameObject.layer == 9 && (a.gameObject.layer == 11 || a.gameObject.layer == 12)) || (gameObject.layer == 11 && (a.gameObject.layer == 9 || a.gameObject.layer == 10)))
        {
            if (!TargetsWithinRange.Contains(a))
            {
                AddLock(a);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger && (other.gameObject.layer == 9 || other.gameObject.layer ==11))
            return;



        EnergySignal Temp = other.GetComponentInParent<EnergySignal>();

        if (!Temp || !Temp.enabled)
            return;
        else
            NewLock(Temp);
        
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
            {
                if (Vector3.Distance(this.transform.position, Temp.transform.position) > RadarRange*0.9)
                {
                    Debug.Log("Peep" + Temp, this);
                    RemoveLock(Temp);
                }
            }
        }
    }

    private void OnDisable()
    {
        if (CurrentPrimary)
        {
            FirePrimary1(false);
            FirePrimary2(false);
        }
        if (CurrentSecondary != null)
        {
            FireSecondary1(false);
            FireSecondary2(false);
        }
    }
}
