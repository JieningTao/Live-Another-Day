using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaptureMTAlert : MonoBehaviour
{
    [SerializeField]
    string Marker;
    [SerializeField]
    string DisableMarker;









    private void CheckDisable(string a,object b)
    {
        if (a == DisableMarker && !(bool)b)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Marker - " + Marker + ": Enter");
            MissionTracker.Instance.UpdateProgress(Marker, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Marker - " + Marker + ": Exit");
            MissionTracker.Instance.UpdateProgress(Marker, false);
        }
    }

    private void OnEnable()
    {
        SceneEventHandler.SceneEventSignal += CheckDisable;
    }

    private void OnDisable()
    {
        SceneEventHandler.SceneEventSignal -= CheckDisable;
    }
}
