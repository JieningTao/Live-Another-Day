using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEActivateReciever : MonoBehaviour
{
    [SerializeField]
    GameObject ThingToActivate;

    [SerializeField]
    string Marker;


    private void SignalRecieved(string a, object b)
    {
        if (a == Marker)
        {
            ThingToActivate.SetActive(true);
        }
    }


    private void OnEnable()
    {
        SceneEventHandler.SceneEventSignal += SignalRecieved;
    }

    private void OnDisable()
    {
        SceneEventHandler.SceneEventSignal -= SignalRecieved;
    }
}
