using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechPartHead : BaseMechPart
{

    BaseMechFCS MyFCS;

    GameObject AimedObject = null;

    [SerializeField]
    Transform Head;

    [SerializeField]
    float TurnSpeed = 1;


    public override void Assemble(BaseMechMain Mech, Transform JointPosition)
    {
        base.Assemble(Mech,JointPosition);
        MyFCS = Mech.GetFCS();
    }


    private void Update()
    {

        if (MyFCS)
        {
            CheckLock();

            AimHead();
        }

    }




    protected void AimHead()
    {
        Vector3 AimDir;

        if (MyFCS.GetMainTarget() != null /*&& Vector3.Angle(MyFCS.transform.forward, MyFCS.GetMainTarget().transform.position - MyFCS.transform.position) < 10*/)
        {
            AimDir = Vector3.RotateTowards(Head.forward, MyFCS.GetMainTarget().transform.position - Head.transform.position, TurnSpeed * Time.deltaTime, 0.0f);
        }
        else
        {
            AimDir = Vector3.RotateTowards(Head.forward, MyFCS.GetLookDirection(), TurnSpeed * Time.deltaTime, 0.0f);
        }

        Head.rotation = Quaternion.LookRotation(AimDir, transform.up);

    }

    private void CheckLock()
    {

        if (MyFCS.GetMainTarget() == null)
        {
            if(AimedObject != null)
                AimedObject = null;
        }
        else
        {
            if(AimedObject != MyFCS.GetMainTarget().gameObject)
                AimedObject = MyFCS.GetMainTarget().gameObject;
        }

    }

}
