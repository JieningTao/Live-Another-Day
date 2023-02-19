using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( BaseMechMain))]
public class AIMechController : MonoBehaviour
{
    [SerializeField]
    Transform TargetWaypointPosition;




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
        MyMech.AIRotate(TargetWaypointPosition.position, 5);
    }
}
