using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHover : MonoBehaviour
{
    [SerializeField]
    Rigidbody MyRB;
    [SerializeField]
    float FloatHeight = 2;
    [SerializeField] //SFT
    float CurrentFloatStrength;
    [SerializeField]
    float FloatRange = 0.5f;
    [SerializeField]
    float RotationForce = 20;
    [SerializeField]
    GameObject Target;

    float GFloat; //Force needed to float under current gravity, inited upon start

    private void Start()
    {
        if (MyRB == null)
            MyRB = GetComponent<Rigidbody>();
        GFloat = -Physics.gravity.y * MyRB.mass;
    }


    private void Update()
    {
        CalculateFloatForce();
        MyRB.AddForce(CurrentFloatStrength*Vector3.up,ForceMode.Force);

        //TestRotate();
        RotateTowardsTarget();
    }


    private void CalculateFloatForce()
    {
        float ForceMultiplier = Mathf.InverseLerp(FloatHeight + FloatRange / 2, FloatHeight - FloatRange / 2, transform.position.y) * 2;
        //Debug.Log(ForceMultiplier);
        CurrentFloatStrength = Mathf.Lerp(-GFloat,3*GFloat,ForceMultiplier);
        CurrentFloatStrength = Mathf.Clamp(CurrentFloatStrength, 0, -Physics.gravity.y * 3 * MyRB.mass);
    }

    private void TestRotate()
    {
        MyRB.AddTorque(new Vector3(0, RotationForce, 0), ForceMode.Force);
    }

    private void RotateTowardsTarget()
    {
        if (!Target)
            return;


        Vector2 From = new Vector2(transform.forward.x, transform.forward.z);

        Vector3 Dir = Target.transform.position - transform.position;

        Vector2 To = new Vector2(Dir.x, Dir.z);

        float Angle = Vector2.SignedAngle(From, To);



        float RotateDirection = 0;

        if (Angle < 0)
        {
            RotateDirection = 1;
            //rotates clockwise
        }
        else if (Angle > 0)
        {
            RotateDirection = -1;
            //rotates anti-clockwise
        }

        float ForceMultiplier = Mathf.InverseLerp(0, 45, Mathf.Abs(Angle));

        float Force = Mathf.Lerp(0.1f * RotationForce, RotationForce, ForceMultiplier);

        MyRB.AddTorque(new Vector3(0, Force * RotateDirection, 0), ForceMode.Force);

        //if (Mathf.Abs(Angle) <= 10)
        //{
        //    //MyRB.AddTorque(new Vector3(0, RotationForce, 0), ForceMode.Force);

        //    if (Angle < 0 && MyRB.angularVelocity.y<0)
        //    {
        //        MyRB.AddTorque(new Vector3(0, RotationForce/2, 0), ForceMode.Force);
        //    }
        //    else if(Angle>0&& MyRB.angularVelocity.y > 0)
        //    {
        //        MyRB.AddTorque(new Vector3(0, -RotationForce/2, 0), ForceMode.Force);
        //    }

        //}
        //else if (Angle < 0)
        //{
        //    MyRB.AddTorque(new Vector3(0, RotationForce, 0), ForceMode.Force);
        //    //rotates clockwise
        //}
        //else if (Angle > 0)
        //{
        //    MyRB.AddTorque(new Vector3(0, -RotationForce, 0), ForceMode.Force);
        //    //rotates anti-clockwise
        //}


        //Debug.Log(Force * RotateDirection + "\n" + MyRB.angularVelocity);

    }
}
