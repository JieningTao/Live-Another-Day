using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveMTAlert : MonoBehaviour
{
    [SerializeField]
    string Marker;

    bool Arrived = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!Arrived && !other.isTrigger && other.GetComponentInParent<PlayerController>())
        {
            MissionTracker.Instance.UpdateProgress(Marker, true);
            Debug.Log("Player arrived at " + gameObject.name);
            Arrived = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (Arrived && !other.isTrigger && other.GetComponentInParent<PlayerController>())
        {
            MissionTracker.Instance.UpdateProgress(Marker, false);
            Debug.Log("Player left " + gameObject.name);
            Arrived = false;
        }
    }
}
