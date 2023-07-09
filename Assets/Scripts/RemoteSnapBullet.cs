using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteSnapBullet : BaseBullet
{
    [HideInInspector]
    [SerializeField]
    BaseShoot Source;
    [SerializeField]
    private ParticleSystem SnapEffect;


    public override void InitBullet(BaseShoot _Source)
    {
        Source = _Source;
        base.InitBullet(_Source);
    }

    private void HandleRemoteWeaponSignal(UnityEngine.Object _Source, string Identifier, object Content)
    {
        if (Source == _Source)
        {
            if (Identifier == "SnapTowards")
            {
                Snap(Content as EnergySignal);
            }
        }
    }

    private void Snap(EnergySignal a)
    {
        if (a != null)
        {
            //Debug.Log(a.GetSpeed());
            Vector3 PredictedLocation = a.GetSpeed() * (Vector3.Distance(transform.position, a.transform.position) / Speed)+a.transform.position;

            Debug.Log(PredictedLocation);
            transform.rotation = Quaternion.LookRotation(PredictedLocation - transform.position, Vector3.up);

            if(SnapEffect)
            Instantiate(SnapEffect.gameObject, transform.position, transform.rotation);
        }

    }



    private void OnEnable()
    {
        RemoteWeaponaryEvents.RemoteWeaponSignal += HandleRemoteWeaponSignal;
    }

    private void OnDisable()
    {
        RemoteWeaponaryEvents.RemoteWeaponSignal -= HandleRemoteWeaponSignal;
    }
}
