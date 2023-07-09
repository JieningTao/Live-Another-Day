using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableSEAlert : MonoBehaviour
{
    [SerializeField]
    string Marker;
    [SerializeField]
    float Amount = 1;



    private void OnDisable()
    {
        Debug.Log("SE Marker - " + Marker + ": " + Amount);
        SceneEventHandler.invokeSES(Marker, Amount);
    }
}
