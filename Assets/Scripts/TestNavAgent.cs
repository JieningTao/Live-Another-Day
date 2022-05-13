using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestNavAgent : MonoBehaviour
{
    NavMeshAgent MyNMA;

    [SerializeField]
    private Transform Target;
         

    private void Start()
    {
        MyNMA = GetComponent<NavMeshAgent>();
    }


    private void Update()
    {
        MyNMA.destination = Target.position;
    }
}
