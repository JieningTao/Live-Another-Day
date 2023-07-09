using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIMNavAgent : AIMovement
{
    private NavMeshAgent _MyNMA;
    private NavMeshAgent MyNMA
    {
        get
        {
            if (_MyNMA == null)
                _MyNMA = GetComponent<NavMeshAgent>();
            return _MyNMA;
        }
    }
    //[SerializeField]
    //private Transform Target;


    public void RecieveTargetPosition(Vector3 Position)
    {
        Target = null;
        MyNMA.destination = Position;
    }

    public void RecieveFollowTarget(Transform _Target)
    {
        Target = _Target;
    }

    public void Spawn(Transform _Target)
    {
        MyNMA.Warp(transform.position);
        RecieveFollowTarget(_Target);
    }

    public void Spawn(Vector3 Position)
    {
        MyNMA.Warp(transform.position);
        RecieveTargetPosition(Position);
    }

    public bool CheckReachable(Vector3 Position)
    {
        NavMeshPath navMeshPath = new NavMeshPath();

        return (MyNMA.CalculatePath(Position, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete);
    }

    public void Stop(bool a)
    {
        MyNMA.isStopped = a;
    }

    private void Update()
    {
        if(Target && MyNMA)
        MyNMA.destination = Target.position;


    }

    private void OnEnable()
    {
        MyNMA.enabled = true;
    }

    private void OnDisable()
    {
        MyNMA.enabled = false;
    }
}
