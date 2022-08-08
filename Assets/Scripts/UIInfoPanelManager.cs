 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInfoPanelManager : MonoBehaviour
{
    [SerializeField]
    private BaseMechMain PlayerMechMain;
    private BaseMechMovement PlayerMechMovement;
    private BaseEnergySource PlayerEnergySystem;
    [SerializeField]
    private float LowWarningThreshhold = 0.2f;
    [SerializeField]
    private float FlashDuration = 0.3f;

    [Space(20)]

    [SerializeField]
    private UnityEngine.UI.Image HealthBar;
    [SerializeField]
    private UnityEngine.UI.Text HealthDisplay;
    [SerializeField]
    private UnityEngine.UI.Text HealthWarning;
    [SerializeField]
    private UnityEngine.UI.Image CoatingBar;

    [Space(20)]

    [SerializeField]
    private UnityEngine.UI.Image EnergyBar;
    [SerializeField]
    private UnityEngine.UI.Text EnergyDisplay;
    [SerializeField]
    private UnityEngine.UI.Text EnergyWarning;

    [Space(20)]

    [SerializeField]
    private UnityEngine.UI.Image BoostBar;
    [SerializeField]
    private UnityEngine.UI.Text BoostDisplay;
    [SerializeField]
    private UnityEngine.UI.Text BoostWarning;

    [Space(20)]

    [SerializeField]
    private UnityEngine.UI.Image SpeedBar;
    [SerializeField]
    private UnityEngine.UI.Text SpeedDisplay;
    [SerializeField]
    private UnityEngine.UI.Text SpeedWarning;

    private List<GameObject> FlashingItems = new List<GameObject>();

    private float CurrentFlashCooldown;
    private bool HWFlashing = false;
    private bool EWFlashing = false;
    private bool BWFlashing = false;
    private bool SWFlashing = false;

    private void Start()
    {
        //UIInitialize();

    }

    public void UIInitialize()
    {
        PlayerMechMovement = PlayerMechMain.GetMovement();
        PlayerEnergySystem = PlayerMechMain.GetEnergySystem();

        HealthWarning.gameObject.SetActive(false);
        EnergyWarning.gameObject.SetActive(false);
        BoostWarning.gameObject.SetActive(false);
        SpeedWarning.gameObject.SetActive(false);
    }


    private void Update()
    { 

        UpdateHealth(PlayerMechMain.GetHealthText(),PlayerMechMain.GetHealthPercent());
        UpdateCoating(PlayerMechMain.GetCoatingPercent());

        UpdateEnergy(PlayerEnergySystem.GetEnergyText(),PlayerEnergySystem.GetEnergyPercent());

        UpdateBoost(PlayerMechMovement.GetBoostText(),PlayerMechMovement.GetBoostPercent());
        UpdateSpeed(PlayerMechMovement.GetSpeedText(),PlayerMechMovement.GetSpeedPercent());
        UpdateFlash();
    }


    private void UpdateHealth(string Text, float Bar)
    {
        HealthDisplay.text = "HP: " + Text;
        HealthBar.fillAmount = Bar;

        if (!HWFlashing && Bar < LowWarningThreshhold)
        {
            HWFlashing = true;
            AddFlashItem(HealthWarning.gameObject);
        }
        else if (HWFlashing && Bar > LowWarningThreshhold)
        {
            HWFlashing = false;
            RemoveFlashItem(HealthWarning.gameObject, false);
        }

        
    }

    private void UpdateCoating(float Bar)
    {
        CoatingBar.fillAmount = Bar;
    }

    private void UpdateEnergy(string Text, float Bar)
    {
        EnergyDisplay.text = "EN: " + Text;
        EnergyBar.fillAmount = Bar;

        if (!EWFlashing && Bar < LowWarningThreshhold)
        {
            EWFlashing = true;
            AddFlashItem(EnergyWarning.gameObject);
        }
        else if (EWFlashing && Bar > LowWarningThreshhold)
        {
            EWFlashing = false;
            RemoveFlashItem(EnergyWarning.gameObject,false);
        }

    }

    private void UpdateBoost(string Text, float Bar)
    {
        BoostDisplay.text = "Boost: " + Text;
        BoostBar.fillAmount = Bar;

        if (!BWFlashing && Bar < LowWarningThreshhold)
        {
            BWFlashing = true;
            AddFlashItem(BoostWarning.gameObject);
        }
        else if (BWFlashing && Bar > LowWarningThreshhold)
        {
            BWFlashing = false;
            RemoveFlashItem(BoostWarning.gameObject,false);
        }
    }

    private void UpdateSpeed(string Text, float Bar)
    {
        SpeedDisplay.text = "Speed: " + Text;
        SpeedBar.fillAmount = Bar;

        if (!SWFlashing && Bar > 0.8f)
        {
            SWFlashing = true;
            AddFlashItem(SpeedWarning.gameObject);
        }
        else if (SWFlashing && Bar <= 0.8f)
        {
            SWFlashing = false;
            RemoveFlashItem(SpeedWarning.gameObject,false);
        }
    }

    private void UpdateFlash()
    {
        if (FlashingItems.Count > 0)
        {
            CurrentFlashCooldown -= Time.deltaTime;
            if (CurrentFlashCooldown <= 0)
            {
                foreach (GameObject a in FlashingItems)
                {
                    a.SetActive(!a.active);
                }
                CurrentFlashCooldown = FlashDuration;
            }
        }
    }

    private void AddFlashItem(GameObject a)
    {
        if (!FlashingItems.Contains(a))
            FlashingItems.Add(a);
    }

    private void RemoveFlashItem(GameObject a, bool ShowAfter)
    {
        if (FlashingItems.Contains(a))
            FlashingItems.Remove(a);

        a.SetActive(ShowAfter);
    }







}
