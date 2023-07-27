using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTurretMK2 : MonoBehaviour
{
    [SerializeField]
    Transform Target;

    [SerializeField]
    Transform AimReference;

    [SerializeField]
    Transform RestAim;

    [SerializeField]
    float TurnSpeed = 1;

    [SerializeField]
    Vector3 TurnLimits = new Vector3(181,181,0);

    [SerializeField]
    Transform HorizontalPart;

    [SerializeField]
    Transform VerticalPart;





    private void Update()
    {
        if (Target)
            AimAt(Target.position);
        else
        {
            Target = RestAim;
            AimAt(Target.position);
        }
    }

    private void AimAt(Vector3 TargetPosition)
    {
        Vector3 AimDir = (TargetPosition - AimReference.position).normalized;
        AimDir = AimReference.InverseTransformDirection(AimDir);
        Quaternion ChangeInRot = Quaternion.LookRotation(AimDir, transform.up);
        Vector3 Euler = ChangeInRot.eulerAngles; // the holy grail, rotations from forward of the aimer, negtive for left and up



        if (Euler.x > 180)
            Euler.x = -(360 - Euler.x);
        if (Euler.y > 180)
            Euler.y = -(360 - Euler.y);
        //if (Euler.z > 10)
        //    Euler.z = -(360 - Euler.z);


        Vector3 TSClamped = Euler;
        TSClamped.x = Mathf.Clamp(TSClamped.x, -TurnSpeed * Time.deltaTime, TurnSpeed * Time.deltaTime);
        TSClamped.y = Mathf.Clamp(TSClamped.y, -TurnSpeed * Time.deltaTime, TurnSpeed * Time.deltaTime);
        //Debug.Log(Euler);

        Vector3 Temp;

        if (HorizontalPart && TSClamped.y != 0)
        {
            HorizontalPart.Rotate(0, TSClamped.y, 0);
            Temp = HorizontalPart.localRotation.eulerAngles;

            if (TurnLimits.y == 0)
                Temp.y = 0;
            else
            {
                if (Temp.y < 180) // special clamping is needed as unity shows rotation as non negative value, eg -10 = 350
                    Temp.y = Mathf.Clamp(Temp.y, -TurnLimits.y, TurnLimits.y);
                else
                    Temp.y = Mathf.Clamp(Temp.y, 360 - TurnLimits.y, 360 + TurnLimits.y);
            }

            HorizontalPart.localRotation = Quaternion.Euler(Temp);
        }

        if (VerticalPart && TSClamped.x != 0)
        {
            VerticalPart.Rotate(TSClamped.x, 0, 0);
            Temp = VerticalPart.localRotation.eulerAngles;
            if (TurnLimits.x == 0)
                Temp.x = 0;
            else
            {
                if (Temp.x < 180) // special clamping is needed as unity shows rotation as non negative value, eg -10 = 350
                    Temp.x = Mathf.Clamp(Temp.x, -TurnLimits.x, TurnLimits.x);
                else
                    Temp.x = Mathf.Clamp(Temp.x, 360 - TurnLimits.x, 360 + TurnLimits.x);
            }

            VerticalPart.localRotation = Quaternion.Euler(Temp);
        }

    }


    public float GetTargetAngleDeviation
    { get { return Vector3.Angle(AimReference.forward, Target.transform.position - AimReference.position); } }
        
    

    public void TurnToRest()
    {
        Target = RestAim;
    }

    public bool IsResting()
    {
        return Target == RestAim.gameObject;
    }
}
