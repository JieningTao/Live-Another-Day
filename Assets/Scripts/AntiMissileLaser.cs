using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiMissileLaser : BaseTurret
{
    [SerializeField]
    SphereCollider MissileDetectCollidder;
    [SerializeField]
    float DetectRange;
    [SerializeField]
    LineRenderer LaserPath;
    [SerializeField]
    float DPS =10;
    [SerializeField]
    float PowerDraw;


    BaseEnergySource EnergySource;
    bool Activated;
    bool LaserOn;
    [SerializeField]
    List<EnergySignal> LockedMissiles = new List<EnergySignal>();
    bool HadTarget = false;
    IDamageable CurrentTargetedMissileDamageable;


    protected override void Update()
    {
        ClearDesstroiedMissiles();

        if (Activated)
        {
            UpdateDecider();

            if (Target != null && !HadTarget)
                ToggleLaser(true);
            else if (Target == null && HadTarget)
                ToggleLaser(false);

            if (Target)
            {
                //needs to do damage to missile
                TurnToTarget(Target.transform.position);
            }
            else
                TurnToTarget(RestAim.transform.position);

            if (LaserOn)
            {
                DamageMissile();
                UpdateLaserPath();
            }

            if (EnergySource.CurrentOutputEffiency < 1)
            {
                Debug.Log(EnergySource.CurrentOutputEffiency);
                ToggleOn(false);
            }

            HadTarget = (Target != null);
        }
        else
        {
            TurnToTarget(RestAim.transform.position);
        }


    }

    private void DamageMissile()
    {
        CurrentTargetedMissileDamageable.Hit(DPS*Time.deltaTime,DamageSystem.DamageType.Energy,new List<DamageSystem.DamageTag>());
    }

    public void InitializeGear(BaseMechMain Mech)
    {
        EnergySource = Mech.GetEnergySystem();
        Debug.Log(EnergySource.gameObject.name);
        gameObject.layer = Mech.gameObject.layer;
        MissileDetectCollidder.radius = DetectRange;
        Debug.Log(EnergySource);
    }

    public void ToggleOn(bool On)
    {
        Debug.Log(gameObject.name + " Toggle " + On);

        Activated = On;

        if (!On)
        {
            Target = null;
            TargetSignal = null;
            ToggleLaser(false);
        }
    }

    protected void UpdateDecider()
    {
        if (!TargetSignal || !TargetSignal.enabled)
        {
            if (LockedMissiles.Count > 0)
            {
                TargetSignal = LockedMissiles[Random.Range(0, LockedMissiles.Count)];
                Target = TargetSignal.gameObject;
                CurrentTargetedMissileDamageable = TargetSignal.GetComponent<IDamageable>();

                return;
            }
        }
        else
            return;

        TargetSignal = null;
        Target = null;
    }


    private void UpdateLaserPath()
    {
        LaserPath.SetPosition(2,transform.InverseTransformPoint(Target.transform.position));
    }

    private void ClearDesstroiedMissiles()
    {
        for (int i = 0; i < LockedMissiles.Count; i++)
        {
            if (LockedMissiles[i] == null)
            {
                LockedMissiles.RemoveAt(i);
                i--;
            }
        }
    }

    private void ToggleLaser(bool On)
    {
        //Debug.Log("Laser " + On);

            LaserOn = On;
        if (On)
        {
            LaserPath.enabled = true;
            if (EnergySource)
            {
            EnergySource.CurrentPowerDraw += PowerDraw;
            }
        }
        else
        {
            LaserPath.enabled = false;
            LaserPath.SetPosition(2, LaserPath.GetPosition(1));
            if(EnergySource)
            EnergySource.CurrentPowerDraw -= PowerDraw;
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        //if (other.isTrigger)
        //    return;

        EnergySignal Temp = other.GetComponentInParent<EnergySignal>();
       
        if (Temp && Temp.MyType == EnergySignal.EnergySignalType.Missile)
        {
            if ((gameObject.layer == 9 &&Temp.gameObject.layer == 12) || (gameObject.layer == 11 && Temp.gameObject.layer == 10))
            {
                if (!LockedMissiles.Contains(Temp))
                    LockedMissiles.Add(Temp);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if (other.isTrigger)
        //    return;

        EnergySignal Temp = GetComponentInParent<EnergySignal>();

        if (Temp)
        {

            if (TargetSignal == Temp)
            {
                TargetSignal = null;
                Target = null;
            }
            LockedMissiles.Remove(Temp);
        }

    }

    public float GetPowerDraw()
    {
        return PowerDraw;
    }

    public bool IsLaserOn
    { get { return LaserOn; } }
    public string GetEnergyDraw
    { get { return PowerDraw+"";} }
    public string GetDPS
    { get { return DPS + "/s"; } }
    public string GetInterceptRange
    { get { return DetectRange + ""; } }

}
