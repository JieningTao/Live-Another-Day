using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMTAlert : MonoBehaviour
{
    [SerializeField]
    string Marker;
    [SerializeField]
    float Amount = 1;



    private void OnDisable()
    {
        Debug.Log("Marker - " + Marker + ": " + Amount);
        MissionTracker.Instance.UpdateProgress(Marker, Amount);
    }
}
