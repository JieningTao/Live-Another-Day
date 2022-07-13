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

    private Transform ObjectToFollow;


    public void RecieveTarget(Transform _Target)
    {
        ObjectToFollow = _Target;
    }

    public void Spawn(Transform _Target)
    {
        MyNMA.Warp(transform.position);
        RecieveTarget(_Target);

    }

    private void Update()
    {
        MyNMA.destination = ObjectToFollow.position;
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
