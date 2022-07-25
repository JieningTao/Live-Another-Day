using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILock : MonoBehaviour
{
    [SerializeField]
    public EnergySignal TrackedSignal;

    [Space(20)]

    [SerializeField]
    UnityEngine.UI.Image RadarEnemyBlip;
    [SerializeField]
    UnityEngine.UI.Image RadarEnemyBlipLock;
    [SerializeField]
    Color RadarBlipLostColor;

    [Space(20)]

    [SerializeField]
    GameObject HUDTracker;
    [SerializeField]
    private UnityEngine.UI.Image HudImage;
    [SerializeField]
    UnityEngine.UI.Text HUDName;
    [SerializeField]
    UnityEngine.UI.Text HUDDistance;
    [SerializeField]
    private UnityEngine.UI.Image HudLock;
    [SerializeField]
    Sprite TargetLostHUDSprite;



    [SerializeField]
    Sprite LowEnergyInRange;
    [SerializeField]
    Sprite LowEnergyOutOfRange;

    [SerializeField]
    Sprite MechInRange;
    [SerializeField]
    Sprite MechOutOfRange;

    private UILockManager MyManager;

    Sprite OutOfRangeSprite;
    Sprite InRangeSprite;

    private Vector3 TargetPosition;
    private float DistanceToTarget = 0;

    private float LockRange;
    private float RadarBlipRangeDelta;
    private bool HUDWasOn;

    private void Update()
    {
        if (TrackedSignal)
            TargetPosition = TrackedSignal.transform.position;

        DistanceToTarget = Vector3.Distance(MyManager.PlayerTransform.position, TargetPosition);

        if (Vector3.Dot(Camera.main.gameObject.transform.forward, (TargetPosition - MyManager.PlayerTransform.position).normalized) >= 0)
        {
            HUDTracker.transform.position = Camera.main.WorldToScreenPoint(TargetPosition);
            HUDDistance.text = (int)DistanceToTarget + "";

            RangeCheck();

            if (!HUDWasOn)
            {
                HUDTracker.SetActive(true);
                HUDWasOn = HUDTracker.active;
            }
        }
        else
        {
            if (HUDWasOn)
            {
                HUDTracker.SetActive(false);
                HUDWasOn = HUDTracker.active;
            }
        }

        MoveRadarBlip();
    }

    public Transform GetHUDTracker()
    {
        return HUDTracker.transform;
    }

    private void MoveRadarBlip()
    {

        Vector3 TempPos = TargetPosition - MyManager.PlayerTransform.position;

        TempPos.y = 0;

        float Temp = TempPos.z;
        TempPos.z = 0;
        TempPos.y = Temp;

        RadarEnemyBlip.transform.localPosition = TempPos * RadarBlipRangeDelta;

        RadarEnemyBlip.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void StartUp(UILockManager _MyManager, float _LockRange, RadarUI RadarParent, EnergySignal Signal)
    {
        TrackedSignal = Signal;
        AssignSprites();
        MyManager = _MyManager;
        RadarEnemyBlip.gameObject.SetActive(true);
        HUDTracker.gameObject.SetActive(true);
        RadarBlipRangeDelta = RadarParent.GetRangeDelta();
        RadarEnemyBlip.transform.parent = RadarParent.RadarBG.transform;
        HUDName.text = TrackedSignal.SignalName;
        LockRange = _LockRange;

        HUDName.gameObject.SetActive(false);
        HUDDistance.gameObject.SetActive(false);

        HUDTracker.SetActive(false);
        HUDWasOn = HUDTracker.active;
    }

    private void AssignSprites()
    {
        if (TrackedSignal.MyType == EnergySignal.EnergySignalType.LowEnergy)
        {
            InRangeSprite = LowEnergyInRange;
            OutOfRangeSprite = LowEnergyOutOfRange;
        }
        else if (TrackedSignal.MyType == EnergySignal.EnergySignalType.Mech)
        {
            InRangeSprite = MechInRange;
            OutOfRangeSprite = MechOutOfRange;
        }

        HudImage.sprite = OutOfRangeSprite;

        //if (DistanceToTarget > LockRange)
        //else if (DistanceToTarget < LockRange)
        //    HudImage.sprite = InRangeSprite;

    }

    private void RangeCheck()
    {
        //only functions when target hasn't been destroied
        if (TrackedSignal)
        {
            if (DistanceToTarget > LockRange && HudImage.sprite == InRangeSprite)
            {
                HUDName.gameObject.SetActive(false);
                HUDDistance.gameObject.SetActive(false);

                HudImage.sprite = OutOfRangeSprite;
            }
            else if (DistanceToTarget < LockRange && HudImage.sprite == OutOfRangeSprite)
            {
                //HUDName.gameObject.SetActive(true);
                //HUDDistance.gameObject.SetActive(true);

                HudImage.sprite = InRangeSprite;
            }
        }
    }

    public void TargetLost()
    {
        //MyManager.m.Remove(this);

        TrackedSignal = null;
        HudImage.sprite = TargetLostHUDSprite;
        HUDName.gameObject.SetActive(true);
        HUDName.text = "Lost";
        HUDDistance.enabled = false;
        RadarEnemyBlip.color = RadarBlipLostColor;

        RadarEnemyBlipLock.gameObject.SetActive(false);
        HudLock.gameObject.SetActive(false);

        Destroy(RadarEnemyBlip.gameObject, 1);
        Destroy(this.gameObject, 1);
    }

    private void CheckTargetLost(string Order, EnergySignal ES)
    {
        if (Order == "Remove")
        {
            if (ES == TrackedSignal)
                TargetLost();
        }
        else if (Order == "Lock")
        {
            if (ES == TrackedSignal)
            {
                HudLock.gameObject.SetActive(true);
                RadarEnemyBlipLock.gameObject.SetActive(true);
            }
        }
        else if (Order == "UnlockAll")
        {
            HudLock.gameObject.SetActive(false);
            RadarEnemyBlipLock.gameObject.SetActive(false);
        }
        else if (Order == "Target")
        {
            //Debug.Log("Targting " + Signal.name);

            if (ES == TrackedSignal)
            {
                HUDName.gameObject.SetActive(true);
                HUDDistance.gameObject.SetActive(true);
            }
            else
            {
                HUDName.gameObject.SetActive(false);
                HUDDistance.gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        BaseMechFCS.LockChanges += CheckTargetLost;
    }

    private void OnDisable()
    {
        BaseMechFCS.LockChanges -= CheckTargetLost;
    }
}
