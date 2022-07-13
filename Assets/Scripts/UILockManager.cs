using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILockManager : MonoBehaviour
{
    [SerializeField]
    Transform MovingCrossHair;
    [SerializeField]
    Transform MovingCrossHairRest;
    [SerializeField]
    float MovingCrossHairSpeed = 50;
    [SerializeField]
    RadarUI RadarParent;
    [SerializeField]
    private GameObject UILockPrefab;
    [SerializeField]
    private GameObject MinorUILockPrefab;
    [SerializeField] // SFT
    private List<UILock> ManagedLocks = new List<UILock>();

    public Transform PlayerTransform;

    private BaseMechFCS PlayerMechFCS;
    [SerializeField]
    private Transform MainTargetUILock;

    private void Start()
    {
        PlayerMechFCS = PlayerTransform.GetComponent<BaseMechFCS>();
        RadarParent.SetRanges(PlayerMechFCS.RadarRange, PlayerMechFCS.LockRange);       
    }

    private void Update()
    {
        MoveCrossHair();
    }

    private void LockChanged(string Order, EnergySignal Signal)
    {
        if (Order == "Add")
            CreateLock(Signal);
        else if (Order == "Target")
        {
            //Debug.Log("Targting " + Signal.name);

            if (Signal == null)
                MainTargetUILock = MovingCrossHairRest;
            else
                MainTargetUILock = GetHUDTransformFromSignal(Signal);
        }

    }

    private Transform GetHUDTransformFromSignal(EnergySignal ES)
    {
        for (int i = 0; i < ManagedLocks.Count; i++)
        {
            //Debug.Log(ManagedLocks[i].TrackedSignal.name + "---" + ES.name);
            if (ManagedLocks[i].TrackedSignal == ES)
                return ManagedLocks[i].GetHUDTracker();

        }
        return null;
    }

    private void CreateLock(EnergySignal Signal)
    {
        if (Signal.MyType == EnergySignal.EnergySignalType.LowEnergy || Signal.MyType == EnergySignal.EnergySignalType.Mech)
        {
            GameObject a = Instantiate(UILockPrefab, transform);
            UILock TempScript = a.GetComponent<UILock>();

            TempScript.StartUp(this, PlayerMechFCS.LockRange, RadarParent, Signal);
            ManagedLocks.Add(TempScript);
        }
        else if (Signal.MyType == EnergySignal.EnergySignalType.Missile || Signal.MyType == EnergySignal.EnergySignalType.HES)
        {
            GameObject a = Instantiate(MinorUILockPrefab, transform);
            MinorUILock TempScript = a.GetComponent<MinorUILock>();

            TempScript.StartUp(this, Signal);
        }

    }

    private void MoveCrossHair()
    {
        if (MainTargetUILock == null)
            MainTargetUILock = MovingCrossHairRest;

        if (MovingCrossHair.position != MainTargetUILock.position)
            MovingCrossHair.position = Vector3.MoveTowards(MovingCrossHair.position, MainTargetUILock.position, MovingCrossHairSpeed * Time.deltaTime);

    }


    private void OnEnable()
    {
        BaseMechFCS.LockChanges += LockChanged;
    }

    private void OnDisable()
    {
        BaseMechFCS.LockChanges -= LockChanged;
    }
}
