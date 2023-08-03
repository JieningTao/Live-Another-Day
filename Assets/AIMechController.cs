using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( BaseMechMain))]
public class AIMechController : MonoBehaviour
{
    [SerializeField]
    Transform TargetWaypointPosition;
    [SerializeField]
    Transform AimReference;
    [SerializeField]
    float TurnSpeed = 1;



    BaseMechMain MyMech;
    BaseMechMovement MyMovement;
    BaseMechFCS MyFCS;


    private void Start()
    {
        MyMech = GetComponent<BaseMechMain>();
        MyFCS = MyMech.GetFCS();
        MyMovement = MyMech.GetMovement();
    }

    // Update is called once per frame
    void Update()
    {
        HandleRotate();
    }

    void HandleRotate()
    {

        Vector3 AimDir = (TargetWaypointPosition.position - transform.position).normalized;
        AimDir = AimReference.InverseTransformDirection(AimDir);
        Quaternion ChangeInRot = Quaternion.LookRotation(AimDir, transform.up);
        Vector3 Euler = ChangeInRot.eulerAngles; // the holy grail, rotations from forward of the aimer, negtive for left and up

        if (Euler.x > 180)
            Euler.x = -(360 - Euler.x);
        if (Euler.y > 180)
            Euler.y = -(360 - Euler.y);

        Vector3 TSClamped = Euler;

        TSClamped.x = Mathf.Clamp(TSClamped.x/50, -50 * Time.deltaTime, 50 * Time.deltaTime);
        TSClamped.y = Mathf.Clamp(TSClamped.y/50, -50 * Time.deltaTime, 50 * Time.deltaTime);

        

        Debug.Log(TSClamped);

        MyMech.Rotate(new Vector3(TSClamped.x, TSClamped.y, 0));
    }
}
