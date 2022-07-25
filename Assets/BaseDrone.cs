using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDrone : BaseEnemy
{

    [Serializable]
    public class AimablePart
    {
        [SerializeField]
        public Transform Part;

        public EnemyGear Weapon;

        [SerializeField]
        public Vector3 WeaponAimLimits = new Vector3(80, 10, 0);

        [SerializeField]
        float TurnSpeed = 2;

        [SerializeField]
        public EnergySignal Target;


        [SerializeField]
        public Vector2 FireInterval;
        protected bool Firing;
        protected float CurrentIntervalCD;

        [SerializeField]
        public float AllowedDeviation = 5;

        [SerializeField]
        public Vector2 FireRange = new Vector2(0, 50);

        public void GetWeapon( BaseEnemy Controller)
        {
            Weapon = Part.GetComponentInChildren<EnemyGear>();
            if(Weapon)
            Weapon.AssignController(Controller);
        }

        protected virtual void AimWeapon(Transform a, Vector3 Dir, Vector3 Limits, float TurnSpeed)
        {
            if (Vector3.Angle(a.forward, Dir) == 0)
                return;

            Vector3 TempDir = Vector3.RotateTowards(a.forward, Dir, TurnSpeed * Time.deltaTime, 0.0f);


            a.rotation = Quaternion.LookRotation(TempDir, a.up);

            //Debug.Log("Ping");

            Vector3 bruh = a.localRotation.eulerAngles; //variable named bruh to commemerate me taking half an hour to realize it wasn't working because turn speed was never changed from initial 0... Fucking idiot.

            bruh.z = 0;

            if (Limits.y == 0)
                bruh.y = 0;
            else
            {
                if (bruh.y < 180)
                    bruh.y = Mathf.Clamp(bruh.y, -Limits.y, Limits.y);
                else
                    bruh.y = Mathf.Clamp(bruh.y, 360 - Limits.y, 360 + Limits.y);
            }

            if (Limits.x == 0)
                bruh.x = 0;
            else
            {
                if (bruh.x < 180)
                    bruh.x = Mathf.Clamp(bruh.x, -Limits.x, Limits.x);
                else
                    bruh.x = Mathf.Clamp(bruh.x, 360 - Limits.x, 360 + Limits.x);
            }

            a.localRotation = Quaternion.Euler(bruh);


        }

        public virtual void AimEmpty()
        {
            AimWeapon(Part, Part.forward, WeaponAimLimits, TurnSpeed);
        }

        public virtual void AimTarget(Vector3 Target)
        {
            AimWeapon(Part, Target - Part.position, WeaponAimLimits, TurnSpeed);
        }



        public virtual void UpdatePart()
        {
            if (Weapon)
            {
                if (Weapon.Aimed)
                {
                    if (Target)
                        AimTarget(Target.transform.position + (Target.GetSpeed() * (Vector3.Distance(Part.transform.position, Target.transform.position) / Weapon.GetBulletSpeed())));
                    else
                        AimEmpty();
                }
                CheckWeaponFire();
            }
        }

        protected virtual void CheckWeaponFire()
        {
            if (CurrentIntervalCD > 0)
            {
                CurrentIntervalCD -= Time.deltaTime;
            }
            else
            {
                if (Firing)
                {
                    Debug.Log("Stop");
                    Weapon.TriggerGear(false);
                    Firing = false;
                    CurrentIntervalCD = FireInterval.y;
                }
                else
                {
                    if (Target)
                    {
                        if (Vector3.Angle(Target.transform.position - Part.transform.position, Part.forward) < AllowedDeviation && Vector3.Distance(Target.transform.position, Part.transform.position) > FireRange.x && Vector3.Distance(Target.transform.position, Part.transform.position) < FireRange.y)
                        {
                            Debug.Log("Start");
                            Weapon.TriggerGear(true);
                            Firing = true;
                            CurrentIntervalCD = FireInterval.x;
                        }
                    }

                }

            }
        }


    }



}
