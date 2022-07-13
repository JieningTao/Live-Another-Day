using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinorUILock : MonoBehaviour
{
    [SerializeField]
    public EnergySignal TrackedSignal;

    [SerializeField]
    private UnityEngine.UI.Image HudImage;

    [SerializeField]
    private Sprite MissileSprite;

    [SerializeField]
    private Sprite HESSprite;


    private UILockManager MyManager;
    private bool HUDWasOn;

    public void StartUp(UILockManager _MyManager, EnergySignal Signal)
    {
        TrackedSignal = Signal;
        AssignSprites();
        MyManager = _MyManager;

    }

    private void Update()
    {
        if (TrackedSignal == null)
        {
            SignalDestroyed();
        }
        else
        {
            if (Vector3.Dot(Camera.main.gameObject.transform.forward, (TrackedSignal.transform.position - MyManager.PlayerTransform.position).normalized) >= 0)
            {
                HudImage.transform.position = Camera.main.WorldToScreenPoint(TrackedSignal.transform.position);

                if (!HUDWasOn)
                {
                    HudImage.gameObject.SetActive(true);
                    HUDWasOn = HudImage.gameObject.active;
                }
            }
            else
            {
                if (HUDWasOn)
                {
                    HudImage.gameObject.SetActive(false);
                    HUDWasOn = HudImage.gameObject.active;
                }
            }
        }



    }

    private void SignalDestroyed()
    {
        Destroy(this.gameObject); 
    }

    private void AssignSprites()
    {
        if (TrackedSignal.MyType == EnergySignal.EnergySignalType.Missile)
        {
            HudImage.sprite = MissileSprite;
        }
        else if (TrackedSignal.MyType == EnergySignal.EnergySignalType.HES)
        {
            HudImage.sprite = HESSprite;
        }
    }
}
