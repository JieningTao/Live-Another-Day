using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneSoldier : BaseDrone
{
    [SerializeField]
    AimablePart LeftWeapon;
    [SerializeField]
    AimablePart RightWeapon;
    [SerializeField]
    Transform AIBody;
    [SerializeField]
    float SelfTurnSpeed;
    [SerializeField]
    Vector2 SurroundRange = new Vector2(10, 50);
    [SerializeField]
    Vector2 RepositionTimeRange = new Vector2(8,20);
    float RepositionCooldown = 0;


    protected override void Start()
    {
        base.Start();
        LeftWeapon.GetWeapon(this);
        RightWeapon.GetWeapon(this);
    }

    protected void Update()
    {
        if(MTargetSignal)
        HandleReposition();

        AimWeapons();
        AimSelf();
    }

    private void HandleReposition()
    {
        RepositionCooldown -= Time.deltaTime;

        if (RepositionCooldown < 0)
        {
            float Dis = Vector3.Distance(transform.position, MTargetSignal.transform.position);
            if (Dis > SurroundRange.x && Dis < SurroundRange.y) 
                Reposition();
            RepositionCooldown = Random.Range(RepositionTimeRange.x, RepositionTimeRange.y);
        }
    }

    private void AimWeapons()
    {
        if(LeftWeapon.Target!=MTargetSignal)
            LeftWeapon.Target = MTargetSignal;
        LeftWeapon.UpdatePart();

        if (RightWeapon.Target != MTargetSignal)
            RightWeapon.Target = MTargetSignal;
        RightWeapon.UpdatePart();
    }


    private void AimSelf()
    {
        if (MTargetSignal)
        {
            AimWeapon(AIBody, MTargetSignal.transform.position - transform.position, new Vector3(0, 360, 0), SelfTurnSpeed);
        }
        else
        {
            AimWeapon(AIBody, transform.forward, new Vector3(0, 360, 0), SelfTurnSpeed);
        }


    }

    private Vector3 GetPositionAroundTarget(Transform a)
    {
        Vector3 Temp;
        AIMNavAgent AIMNA = MyMovement as AIMNavAgent;

        for (int i = 0; i < 5; i++) //limit the max amount of tries that navagent will try to find a reachable position
        {
            Temp = a.position;
            Temp += new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized * Random.Range(SurroundRange.x, SurroundRange.y);

            Debug.DrawLine(Temp, Temp + new Vector3(0, 10, 0), Color.cyan, 10);

            //if (AIMNA.CheckReachable(Temp))
            {
                return Temp;

                AIMNA.RecieveTargetPosition(Temp);
            }
        }
        return a.position;
    }

    private void Reposition()
    {
        Vector3 a = GetPositionAroundTarget(MTargetSignal.transform);
        Debug.DrawLine(a, a + new Vector3(0, 10, 0), Color.red, 10);
        (MyMovement as AIMNavAgent).RecieveTargetPosition(a);
    }


}
