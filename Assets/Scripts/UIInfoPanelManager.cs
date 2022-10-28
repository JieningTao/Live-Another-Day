 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInfoPanelManager : MonoBehaviour
{
    [SerializeField]
    private BaseMechMain PlayerMechMain;
    private BaseMechFCS PlayerMechFCS;
    private BaseMechMovement PlayerMechMovement;
    private BaseEnergySource PlayerEnergySystem;

    [Space(20)]
    [SerializeField]
    private UIBarHealthController HPDisplay;
    [SerializeField]
    private UIBarDisplayController EnergyDisplay;
    [SerializeField]
    private UIBarLockController LockDisplay;
    [SerializeField]
    private UIBarDisplayController AmmoDisplay;
    [Space(10)]
    [SerializeField]
    private UIBarSpeedController SpeedDisplay;
    [SerializeField]
    private UIBarDisplayController BoostDisplay;


    //[SerializeField]
    //private UnityEngine.UI.Image HealthBar;
    //[SerializeField]
    //private UnityEngine.UI.Text HealthDisplay;
    //[SerializeField]
    //private UnityEngine.UI.Text HealthWarning;
    //[SerializeField]
    //private UnityEngine.UI.Image CoatingBar;

    //[Space(20)]

    //[SerializeField]
    //private UnityEngine.UI.Image EnergyBar;
    //[SerializeField]
    //private UnityEngine.UI.Text EnergyDisplay;
    //[SerializeField]
    //private UnityEngine.UI.Text EnergyWarning;

    //[Space(20)]

    //[SerializeField]
    //private UnityEngine.UI.Image BoostBar;
    //[SerializeField]
    //private UnityEngine.UI.Text BoostDisplay;
    //[SerializeField]
    //private UnityEngine.UI.Text BoostWarning;

    //[Space(20)]

    //[SerializeField]
    //private UnityEngine.UI.Image SpeedBar;
    //[SerializeField]
    //private UnityEngine.UI.Text SpeedDisplay;
    //[SerializeField]
    //private UnityEngine.UI.Text SpeedWarning;

    private void Start()
    {
        //UIInitialize();

    }

    public void UIInitialize()
    {
        if (!PlayerMechMain)
        {
            PlayerMechMain = FindObjectOfType<PlayerController>().GetComponent<BaseMechMain>();
        }

        PlayerMechMovement = PlayerMechMain.GetMovement();
        PlayerMechFCS = PlayerMechMain.GetFCS();
        PlayerEnergySystem = PlayerMechMain.GetEnergySystem();

        HPDisplay.UIInitialize();
        EnergyDisplay.UIInitialize();
        SpeedDisplay.UIInitialize(PlayerMechMovement.GetNormSpeedLimitRatio());
        BoostDisplay.UIInitialize();
        AmmoDisplay.UIInitialize();
        LockDisplay.Initilize(PlayerMechFCS);


    }


    private void Update()
    {

        //regular bar displays
        HPDisplay.UpdateBar(PlayerMechMain.GetHealthText(), PlayerMechMain.GetHealthPercent(), PlayerMechMain.GetCoatingPercent());
        EnergyDisplay.UpdateBar(PlayerEnergySystem.GetEnergyText(), PlayerEnergySystem.GetEnergyPercent());
        BoostDisplay.UpdateBar(PlayerMechMovement.GetBoostText(), PlayerMechMovement.GetBoostPercent());
        //LockDisplay.UpdateLockNumbers(PlayerMechFCS.GetLockedAmount());
        //circle displays
        SpeedDisplay.UpdateBar(PlayerMechMovement.GetSpeedText(), PlayerMechMovement.GetSpeedPercent());
    }


    //private void UpdateCoating(float Bar)
    //{
    //    CoatingBar.fillAmount = Bar;
    //}

    //private void UpdateEnergy(string Text, float Bar)
    //{
    //    EnergyDisplay.text = "EN: " + Text;
    //    EnergyBar.fillAmount = Bar;

    //    if (!EWFlashing && Bar < LowWarningThreshhold)
    //    {
    //        EWFlashing = true;
    //        AddFlashItem(EnergyWarning.gameObject);
    //    }
    //    else if (EWFlashing && Bar > LowWarningThreshhold)
    //    {
    //        EWFlashing = false;
    //        RemoveFlashItem(EnergyWarning.gameObject,false);
    //    }

    //}

    //private void UpdateBoost(string Text, float Bar)
    //{
    //    BoostDisplay.text = "Boost: " + Text;
    //    BoostBar.fillAmount = Bar;

    //    if (!BWFlashing && Bar < LowWarningThreshhold)
    //    {
    //        BWFlashing = true;
    //        AddFlashItem(BoostWarning.gameObject);
    //    }
    //    else if (BWFlashing && Bar > LowWarningThreshhold)
    //    {
    //        BWFlashing = false;
    //        RemoveFlashItem(BoostWarning.gameObject,false);
    //    }
    //}

    //private void UpdateSpeed(string Text, float Bar)
    //{
    //    SpeedDisplay.text = /*"Speed: " +*/ Text;
    //    SpeedBar.fillAmount = Bar;

    //    if (!SWFlashing && Bar > 0.8f)
    //    {
    //        SWFlashing = true;
    //        AddFlashItem(SpeedWarning.gameObject);
    //    }
    //    else if (SWFlashing && Bar <= 0.8f)
    //    {
    //        SWFlashing = false;
    //        RemoveFlashItem(SpeedWarning.gameObject,false);
    //    }
    //}

    //private void UpdateFlash()
    //{
    //    if (FlashingItems.Count > 0)
    //    {
    //        CurrentFlashCooldown -= Time.deltaTime;
    //        if (CurrentFlashCooldown <= 0)
    //        {
    //            foreach (GameObject a in FlashingItems)
    //            {
    //                a.SetActive(!a.active);
    //            }
    //            CurrentFlashCooldown = FlashDuration;
    //        }
    //    }
    //}

    //private void AddFlashItem(GameObject a)
    //{
    //    if (!FlashingItems.Contains(a))
    //        FlashingItems.Add(a);
    //}

    //private void RemoveFlashItem(GameObject a, bool ShowAfter)
    //{
    //    if (FlashingItems.Contains(a))
    //        FlashingItems.Remove(a);

    //    a.SetActive(ShowAfter);
    //}







}
