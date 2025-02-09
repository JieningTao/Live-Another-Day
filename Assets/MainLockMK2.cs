using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainLockMK2 : MonoBehaviour
{

    [SerializeField]
    List<LockProfile> Profiles = new List<LockProfile>(5);
    [Serializable]
    public class LockProfile
    {
        [SerializeField]
        public MainLockEnergySignal.MLSignalType ProfileSignalType;
        [SerializeField]
        public Sprite InRange;
        [SerializeField]
        public Sprite OutOfRange;
        [SerializeField]
        public Sprite Radar;
    }
    [Space(20)]
    [Header("Hud Settings")]
    [SerializeField]
    Image HUDImage;
    [SerializeField]
    Sprite SigLosSprite;
    [SerializeField]
    Animator LockAnim;
    [SerializeField]
    Image Health;
    [SerializeField]
    Text HUDName;
    [SerializeField]
    Text HUDDistance;

    [Space(20)]
    [Header("Radar Blip")]
    [SerializeField]
    Image RadarIcon;
    [SerializeField]
    Color LOSColor;
    [SerializeField]
    Image RadarLockIcon;
    [SerializeField]
    Image RadarAppearIcon;

    [Space(20)]
    [Header("Other Settings")]
    [SerializeField]
    float StayAfterLOS = 1;
    float DisappearCD;


    [SerializeField] //SFT
    public EnergySignal TrackedSignal;
    private IDamageable TrackedHealth;

    private Vector3 TargetPosition;
    private float DistanceToTarget = 0;

    private float LockRange;
    private float RadarBlipRangeDelta;
    private bool HUDWasOn;
    private int CurrentLockProfile;
    private Transform PlayerTransform;
    bool Disappearing = false;


    public void Init(MainLockEnergySignal.MLSignalType ST,float _LockRange,Transform Player,EnergySignal _TrackedSignal,RadarUI RadarParent)
    {
        

        PlayerTransform = Player;
        TrackedSignal = _TrackedSignal;
        LockRange = _LockRange;
        RadarBlipRangeDelta = RadarParent.GetRangeDelta();
        RadarIcon.transform.parent = RadarParent.RadarBG.transform;

        for (int i = 0; i < Profiles.Count; i++)
        {
            if (Profiles[i].ProfileSignalType == ST)
            {
                CurrentLockProfile = i;
                RangeCheck(); //needs to happen after player and tracked signal is set so ranging doesn't bug out

                Debug.Log(CurrentLockProfile);

                RadarIcon.sprite = Profiles[CurrentLockProfile].Radar;
            }
        }
        Disappearing = false;

        gameObject.SetActive(true);
        RadarIcon.gameObject.SetActive(true);
    }

    private void Update()
    {

        HandleDisappear();

        if (TrackedSignal)
            TargetPosition = TrackedSignal.transform.position;

        DistanceToTarget = Vector3.Distance(PlayerTransform.position, TargetPosition);

        if (Vector3.Dot(Camera.main.gameObject.transform.forward, (TargetPosition - PlayerTransform.position).normalized) >= 0)
        {
            transform.position = Camera.main.WorldToScreenPoint(TargetPosition);
            HUDDistance.text = (int)DistanceToTarget + "";

            RangeCheck();

            if (!HUDWasOn)
            {
                HUDImage.gameObject.SetActive(true);
                HUDWasOn = HUDImage.gameObject.active;
            }
        }
        else
        {
            if (HUDWasOn)
            {
                HUDImage.gameObject.SetActive(false);
                HUDWasOn = HUDImage.gameObject.active;
            }
        }

        MoveRadarBlip();
    }


    private void HandleDisappear()
    {
        if (Disappearing)
        {
            DisappearCD -= Time.deltaTime;
            if (DisappearCD <= 0)
                CompleteDisappear();
        }
    }

    private void RangeCheck()
    {
        //only functions when target hasn't been destroied
        if (TrackedSignal)
        {
            if (DistanceToTarget > LockRange && HUDImage.sprite != Profiles[CurrentLockProfile].OutOfRange)
            {
                HUDName.gameObject.SetActive(false);
                HUDDistance.gameObject.SetActive(false);

                HUDImage.sprite = Profiles[CurrentLockProfile].OutOfRange;
            }
            else if (DistanceToTarget < LockRange && HUDImage.sprite != Profiles[CurrentLockProfile].InRange)
            {
                //HUDHUDName.gameObject.SetActive(true);
                //HUDDistance.gameObject.SetActive(true);

                HUDImage.sprite = Profiles[CurrentLockProfile].InRange;
            }
        }
    }


    private void MoveRadarBlip()
    {

        Vector3 TempPos = TargetPosition - PlayerTransform.position;

        TempPos.y = 0;

        float Temp = TempPos.z;
        TempPos.z = 0;
        TempPos.y = Temp;

        RadarIcon.transform.localPosition = TempPos * RadarBlipRangeDelta;

        RadarIcon.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public void TargetLost()
    {
        //MyManager.m.Remove(this);

        TrackedSignal = null;
        HUDImage.sprite = SigLosSprite;
        HUDName.gameObject.SetActive(true);
        HUDName.text = "Lost";
        HUDDistance.enabled = false;
        RadarIcon.color = LOSColor;

        RadarLockIcon.gameObject.SetActive(false);
        LockAnim.SetBool("Locked", false);

        Disappearing = true;
        DisappearCD = StayAfterLOS;
    }

    private void CompleteDisappear()
    {
        RadarLockIcon.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void UpdateLockChanges(string Order, EnergySignal ES)
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
                LockAnim.SetBool("Locked", true);
                RadarLockIcon.gameObject.SetActive(true);
            }
        }
        else if (Order == "UnlockAll")
        {
            LockAnim.SetBool("Locked", false);
            RadarLockIcon.gameObject.SetActive(false);
        }
        else if (Order == "MainTarget")
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
        BaseMechFCS.LockChanges += UpdateLockChanges;
    }

    private void OnDisable()
    {
        BaseMechFCS.LockChanges -= UpdateLockChanges;
    }


}
