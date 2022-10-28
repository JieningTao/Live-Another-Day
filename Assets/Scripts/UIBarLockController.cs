using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBarLockController : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Image LockSymbol;
    [SerializeField]
    Color StandbyColor;
    [SerializeField]
    Color LockingColor;
    [SerializeField]
    Color LockFullColor;
    [SerializeField]
    Color LockUnavaliableColor;

    [Space(20)]

    [SerializeField]
    UnityEngine.UI.Text Title;
    [SerializeField]
    UnityEngine.UI.Text LockNum;
    [SerializeField]
    UnityEngine.UI.Text MaxLocks;

    [Space(20)]

    [SerializeField]
    protected GameObject Warning;
    [SerializeField]
    protected float LowWarningThreshhold = 0.2f;
    [SerializeField]
    protected float FlashDuration = 0.3f;
    protected float CurrentFlashCooldown;
    protected bool WarningFlashing = false;
    BaseMechFCS PlayerFCS;






    protected void Update()
    {
        UpdateFlash();
    }

    public void Initilize(BaseMechFCS FCS)
    {
        PlayerFCS = FCS;
        LockStandby();
    }

    public void UpdateLock()
    {
        LockNum.text = PlayerFCS.GetLockedAmount() + "";
    }

    #region StateChangers
    private void NewStartLock(string Name)
    {
        Title.text = Name;
        MaxLocks.text = PlayerFCS.GetCurrentRequestedAmount() + " | " + PlayerFCS.GetMaxLockAmount();
        LockSymbol.color = LockingColor;
    }

    private void MaxLockReached()
    {
        Flash(true);
        LockSymbol.color = LockFullColor;
    }

    private void LockStandby()
    {
        Title.text = "";
        MaxLocks.text = "0 | " + PlayerFCS.GetMaxLockAmount();
        Flash(false);
        LockSymbol.color = StandbyColor;
        UpdateLock();
    }
    #endregion

    protected void UpdateFlash()
    {
        if (WarningFlashing)
        {
            CurrentFlashCooldown -= Time.deltaTime;
            if (CurrentFlashCooldown <= 0)
            {
                Warning.SetActive(!Warning.active);
                CurrentFlashCooldown = FlashDuration;
            }
        }
    }

    protected void Flash(bool Start)
    {
        if (Start)
        {
            WarningFlashing = true;
            Warning.SetActive(true);
            CurrentFlashCooldown = FlashDuration;
        }
        else
        {
            WarningFlashing = false;
            Warning.SetActive(false);
            CurrentFlashCooldown = FlashDuration;
        }
    }

    private void UpdateLockChanges(string Order, EnergySignal ES)
    {
        if (Order == "ClearLockRequester"|| Order == "UnlockAll")
            LockStandby();
        else if (Order == "NewLockRequester")
            NewStartLock(PlayerFCS.GetLockRequesterName());
        else if (Order == "LockAtMax")
            MaxLockReached();
        else if (Order == "Lock")
            UpdateLock();
        

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
