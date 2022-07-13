using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationTravelMount : MonoBehaviour
{
    [SerializeField]
    private float Delay = 0;

    [SerializeField]
    private float Speed;
    [SerializeField]
    private float TurnFactor;


    private Transform NextNode;
    private NodeManager MyManager;
    private List<ConvoyFormationSpawner> AllSpawners;
    private bool Moving = false;


    private void Start()
    {
        MyManager = GetComponentInParent<NodeManager>();

        transform.position = MyManager.GetFirstNode().position;


        NextNode = MyManager.GetInitialNode();

        transform.rotation = Quaternion.LookRotation(NextNode.position - transform.position, this.transform.up);

        
    }

    private void Update()
    {
        if (!Moving)
        {
            Delay -= Time.deltaTime;
            if (Delay <= 0)
                StartUp();
        }

        if (Moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, NextNode.position, Speed * Time.deltaTime);

            if (transform.forward != (NextNode.position - transform.position).normalized)
            {

                Vector3 BaseDir = Vector3.RotateTowards(transform.forward, NextNode.position - transform.position, TurnFactor * Speed * Mathf.Deg2Rad * Time.deltaTime, 0.0f);

                transform.rotation = Quaternion.LookRotation(BaseDir, this.transform.up);
            }


            if (Vector3.Distance(transform.position, NextNode.position) < 1)
                GetNextNode();
        }





    }

    private void StartUp()
    {
        Moving = true;

        AllSpawners = new List<ConvoyFormationSpawner>();
        AllSpawners.AddRange(GetComponentsInChildren<ConvoyFormationSpawner>());

        foreach (ConvoyFormationSpawner a in AllSpawners)
        {
            a.StartUp();
        }

    }

    private void GetNextNode()
    {
        NextNode = MyManager.GetNextNode(NextNode);
    }


}
