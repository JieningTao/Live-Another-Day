using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILockManager : MonoBehaviour
{

    public static UILockManager _instance;
    public static UILockManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UILockManager>();
            }
            return _instance;
        }
    }

    [SerializeField]
    Transform CrossHairRest;
    [SerializeField]
    Transform MainCrossHair;
    [SerializeField]
    Transform EXGCrossHair;
    [SerializeField]
    float MovingCrossHairSpeed = 500;
    [SerializeField]
    RadarUI RadarParent;

    [Space(20)]

    [SerializeField]
    private GameObject UILockPrefab;
    [SerializeField]
    private GameObject MinorUILockPrefab;
    [SerializeField]
    private GameObject ObjectiveUILockPrefab;

    [Space(20)]

    //[SerializeField] // SFT
    private List<UILock> ManagedLocks = new List<UILock>();

    private List<MissionPointLockTracker> ManagedMissionTrackers = new List<MissionPointLockTracker>();

    public Transform PlayerTransform;

    private BaseMechFCS PlayerMechFCS;
    [SerializeField]
    private Transform MainTargetUILock;
    [SerializeField]
    private Transform EXGTargetUILock;

    private void Start()
    {
        if (!PlayerTransform)
        {
            PlayerTransform = FindObjectOfType<PlayerController>().transform;
        }

        PlayerMechFCS = PlayerTransform.GetComponent<BaseMechFCS>();
        RadarParent.SetRanges(PlayerMechFCS.RadarRange, PlayerMechFCS.LockRange);       
    }

    private void Update()
    {
        MoveCrossHairs();
    }

    private void LockChanged(string Order, EnergySignal Signal)
    {
        //Debug.Log(Order);

        if (Order == "Add")
            CreateLock(Signal);
        else if (Order == "MainTarget")
        {
            //Debug.Log("Targting " + Signal.name);

            if (Signal == null)
                MainTargetUILock = CrossHairRest;
            else
                MainTargetUILock = GetHUDTransformFromSignal(Signal);
        }
        else if (Order == "EXGReticleOn")
            EXGCrossHair.gameObject.SetActive(true);
        else if (Order == "EXGReticleOff")
            EXGCrossHair.gameObject.SetActive(false);
        else if (Order == "EXGLock")
        {
            if (Signal == null)
                EXGTargetUILock = CrossHairRest;
            else
                EXGTargetUILock = GetHUDTransformFromSignal(Signal);
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

    public void CreateMissionTracker(string Title, GameObject FollowObject)
    {
        GameObject a = Instantiate(ObjectiveUILockPrefab, transform);
        MissionPointLockTracker TempScript = a.GetComponent<MissionPointLockTracker>();
        ManagedMissionTrackers.Add(TempScript);

        TempScript.Create(this, RadarParent, FollowObject, Title);
    }

    public void ClearMissionTrackers()
    {
        for (int i = 0; i < ManagedMissionTrackers.Count; i++)
        {
            Destroy(ManagedMissionTrackers[i].gameObject);
        }
        ManagedMissionTrackers.Clear();
    }

    private void MoveCrossHairs()
    {
        if (MainTargetUILock == null)
            MainTargetUILock = CrossHairRest;

        if (MainCrossHair.position != MainTargetUILock.position)
            MainCrossHair.position = Vector3.MoveTowards(MainCrossHair.position, MainTargetUILock.position, MovingCrossHairSpeed * Time.deltaTime);

        if (EXGCrossHair.gameObject.active)
        {
            if (EXGTargetUILock == null)
                EXGTargetUILock = CrossHairRest;

            if (EXGCrossHair.position != EXGTargetUILock.position)
                EXGCrossHair.position = Vector3.MoveTowards(EXGCrossHair.position, EXGTargetUILock.position, MovingCrossHairSpeed * Time.deltaTime);
        }
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
