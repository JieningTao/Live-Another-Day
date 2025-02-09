using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLockManagerMK2 : MonoBehaviour
{

    public static MainLockManagerMK2 _instance;
    public static MainLockManagerMK2 Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MainLockManagerMK2>();
            }
            return _instance;
        }
    }

    [SerializeField]
    MainLockMK2 LockPrefab;
    List<MainLockMK2> PooledLocks = new List<MainLockMK2>();
    [SerializeField]
    int InitialLoadCount;
    [SerializeField]
    RadarUI RadarParent;

    public Transform PlayerTransform;
    private BaseMechFCS PlayerMechFCS;

    public void Initialize()
    {
        if (!PlayerTransform)
        {
            PlayerTransform = FindObjectOfType<PlayerController>().transform;
        }

        PlayerMechFCS = PlayerTransform.GetComponent<BaseMechFCS>();
        RadarParent.SetRanges(PlayerMechFCS.RadarRange, PlayerMechFCS.LockRange);
    }

    private void Start()
    {
        PooledLocks = new List<MainLockMK2>();

        for (int i = 0; i < InitialLoadCount; i++)
        {
            MainLockMK2 Temp = Instantiate(LockPrefab, transform);
            Temp.gameObject.SetActive(false);
            PooledLocks.Add(Temp);

        }
    }

    public MainLockMK2 GetUILock()
    {
        for (int i = 0; i < PooledLocks.Count; i++)
        {
            if (!PooledLocks[i].gameObject.active)
                return PooledLocks[i];
        }
        MainLockMK2 Temp = Instantiate(LockPrefab, transform);
        return Temp;
    }

    private void CreateLock(EnergySignal Signal)
    {
        Debug.Log("CL");

        if (Signal is MainLockEnergySignal)
        {
            MainLockMK2 A = GetUILock();

            A.Init((Signal as MainLockEnergySignal).MySignalType, PlayerMechFCS.LockRange,PlayerTransform,Signal,RadarParent);
        }
        //needs to filter and do stuff for minorlock


    }

    private void LockChanged(string Order, EnergySignal Signal)
    {
        //Debug.Log(Order);

        if (Order == "Add")
            CreateLock(Signal);
        //else if (Order == "MainTarget")
        //{
        //    //Debug.Log("Targting " + Signal.name);

        //    if (Signal == null)
        //        MainTargetUILock = CrossHairRest;
        //    else
        //        MainTargetUILock = GetHUDTransformFromSignal(Signal);
        //}
        //else if (Order == "EXGReticleOn")
        //    EXGCrossHair.gameObject.SetActive(true);
        //else if (Order == "EXGReticleOff")
        //    EXGCrossHair.gameObject.SetActive(false);
        //else if (Order == "EXGLock")
        //{
        //    if (Signal == null)
        //        EXGTargetUILock = CrossHairRest;
        //    else
        //        EXGTargetUILock = GetHUDTransformFromSignal(Signal);
        //}



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
