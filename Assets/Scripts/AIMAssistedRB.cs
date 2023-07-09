using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMAssistedRB : AIMovement
{
    [SerializeField]
    Vector3 TargetLoaction;
    [SerializeField]
    Rigidbody MyRB;
    [SerializeField]
    float MoveForce = 5;
    [SerializeField]
    float SpeedCap =8;
    [SerializeField]
    float SurroundDistance=0;
    [SerializeField]
    bool CarMovement;
    [SerializeField]
    float TurnSpeed;

    [SerializeField]
    float PathCalculateCD = 1;
    float PathCalculateTimer;
    [SerializeField]
    float PathDeviation = 1;

    [SerializeField]
    private LayerMask GroundDetection;
    [SerializeField]
    private Transform GroundDetectionSite;
    [SerializeField]
    private float GroundDetectionRadius = 0.5f;
    RaycastHit GroundUnder;

    Vector3 LastTargetPosition;
    NavMeshPath CurrentPath;
    List<Vector3> PathNodes = new List<Vector3>();
    Vector3 MoveDirection;

    private void Start()
    {
        if(Target)
        CalculatePath(Target.position);
    }
    // Update is called once per frame
    void Update()
    {

        Drawpath();
        PathCalculateTimer += Time.deltaTime;

        CheckToRecalculate();
        DirectAlongPath();


        if (SurroundDistance > 0)
        {


                if (Target&&Vector3.Distance(Target.position, transform.position) < SurroundDistance && PathNodes.Count <= 1)
                {
                    //when have sight on target and is within surround distance, stop approaching
                }
                else
                    Move();
            
        }
        else
            Move();


        CheckAndCapSpeed();

    }

    void Drawpath()
    {
        if (PathNodes != null && PathNodes.Count > 0)
        {
            for (int i = 0; i < PathNodes.Count; i++)
            {
                if (i == 0)
                    Debug.DrawLine(transform.position, PathNodes[0], Color.magenta);
                else
                    Debug.DrawLine(PathNodes[i-1], PathNodes[i], Color.cyan);
            }
        }
    }

    public void RecieveTargetPosition(Vector3 Position)
    {
        Target = null;
        CalculatePath(Position);
    }

    public void RecieveFollowTarget(Transform a, float FollowDistance)
    {
        Target = a;
        SurroundDistance = FollowDistance;
    }



    void CheckAndCapSpeed()
    {
        if (MyRB.velocity.magnitude > SpeedCap)
        {
            MyRB.velocity = MyRB.velocity.normalized * SpeedCap;
        }
    }

    void DirectAlongPath()
    {
        if (Hold)
            return;
        if (PathNodes.Count < 1)
        {
            MoveDirection = Vector3.zero;
            return;
        }

        Vector3 Direction;
        Direction = GetNextNodeInPath() - transform.position;

        Direction.y = 0;
        Direction.Normalize();

        MoveDirection = Direction;

        


    }

    void Move()
    {
        if (PathNodes.Count < 1)
        {
            //do not move if there is no target
        }
        else
        {
            if (CarMovement)
                VerhicleMove();
            else
                HoverMove();
        }

    }

    void HoverMove()
    {
        //Debug.Log("Moving");
        Vector3 ProjectedInput = Vector3.ProjectOnPlane(MoveDirection, GroundUnder.normal).normalized;

        MyRB.AddForce(ProjectedInput * MoveForce, ForceMode.Force);
    }

    void VerhicleMove()
    {


        Vector3 TempDir = Vector3.RotateTowards(transform.forward, MoveDirection, TurnSpeed * Time.deltaTime, 0.0f);
        transform.rotation = Quaternion.LookRotation(TempDir, transform.up);



        float Angle = Vector3.Angle(transform.forward, MoveDirection);
        if (Angle < 60)
        {
            Vector3 ProjectedInput = Vector3.ProjectOnPlane(transform.forward, GroundUnder.normal).normalized;
            MyRB.AddForce(ProjectedInput * MoveForce, ForceMode.Force);
        }
        else if (Angle > 120)
        {
            Vector3 ProjectedInput = Vector3.ProjectOnPlane(-transform.forward, GroundUnder.normal).normalized;
            MyRB.AddForce(ProjectedInput * MoveForce, ForceMode.Force);
        }

    }

    Vector3 GetNextNodeInPath()
    {
        if (PathNodes.Count > 0)
        {
            while (Vector3.Distance(GetPositionOnGround, PathNodes[0]) < PathDeviation && PathNodes.Count > 1)
            {
                PathNodes.RemoveAt(0);
            }
        }
            return PathNodes[0];


        //for (int i = 1; i < CurrentPath.corners.Length; i++)
        //{
        //    if (Vector3.Distance(GetPositionOnGround, CurrentPath.corners[i]) > PathDeviation)
        //        return CurrentPath.corners[i];
        //}
        //return CurrentPath.corners[0];
    }

    bool CalculatePath(Vector3 TargetPosition)
    {
        Vector3 GP = GetPositionOnGround;
        Vector3 TP = GetPointPositionOnNavmesh(TargetPosition);

        //Debug.Log("Calculating Path");

        if (GP == Vector3.zero || TP == Vector3.zero)
        {
            Debug.Log("PC failed");
            Debug.Log("GP "+GP);
            Debug.Log("TP "+TP);
            return false;
        }


        CurrentPath = new NavMeshPath();

        //DO NOT for the love of Starclan, disable this line, it FUCKS the whole nav system
        Debug.Log(NavMesh.CalculatePath(GP, TP, NavMesh.AllAreas, CurrentPath));

        PathNodes.Clear();

        foreach (Vector3 a in CurrentPath.corners)
        {
            PathNodes.Add(a);
        }


        PathCalculateTimer = 0;
        return true;
    }

    void CheckToRecalculate()
    {
        if (PathCalculateTimer < PathCalculateCD) // does not reacalculate in cooldown period
            return;

        if (Target)
        {
            if (Target.position != LastTargetPosition)
            {
                CalculatePath(Target.position);
            }
        }

    }

    Vector3 GetTargetPositionOnNavMesh
    {
        get
        {
            return GetPointPositionOnNavmesh(Target.position);
        }
    }

    Vector3 GetPointPositionOnNavmesh(Vector3 a)
    {
        RaycastHit Hit;

        if (Physics.Raycast(a, -transform.up, out Hit, 100, GroundDetection))
            return Hit.point;

        return Vector3.zero;
    }

    Vector3 GetPositionOnGround
    {
        get {
            if( Grounded())
                return GroundUnder.point;
            return Vector3.zero;
        }
    }

    public bool CheckReachable(Vector3 TargetPosition)
    {
        Vector3 GP = GetPositionOnGround;
        Vector3 TP = GetPointPositionOnNavmesh(TargetPosition);

        if (GP == Vector3.zero || TP == Vector3.zero)
        {
            Debug.Log("PC failed");
            Debug.Log("GP " + GP);
            Debug.Log("TP " + TP);
            return false;
        }


        CurrentPath = new NavMeshPath();
        if (NavMesh.CalculatePath(GP, TP, NavMesh.AllAreas, CurrentPath))
            return true;

        return false;
    }

    public bool Grounded()
    {
        if (Physics.Raycast(GroundDetectionSite.position, -transform.up, out GroundUnder, GroundDetectionRadius, GroundDetection))
        {
            //float Angle = Vector3.Angle(transform.up, GroundUnder.normal); //gets angle of slope
            return true;
        }
        return false;
    }

}
