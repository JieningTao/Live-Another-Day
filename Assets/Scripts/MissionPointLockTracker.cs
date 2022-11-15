using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionPointLockTracker : MonoBehaviour
{
    private GameObject TrackedObject;


    [SerializeField]
    UnityEngine.UI.Image RadarTrackBlip;
    [SerializeField]
    GameObject HUDTracker;
    [SerializeField]
    public UnityEngine.UI.Text Title;
    [SerializeField]
    public UnityEngine.UI.Text Distance;

    private float RadarBlipRangeDelta;
    private UILockManager MyManager;
    private bool HUDWasOn = false;
    private Vector3 TargetPosition;
    private float DistanceToTarget;

    public void Create(UILockManager Manager, RadarUI RadarParent, GameObject _TrackedObject, string _Title)
    {
        MyManager = Manager;
        TrackedObject = _TrackedObject;
        Title.text = _Title;
        RadarBlipRangeDelta = RadarParent.GetRangeDelta();
        RadarTrackBlip.transform.SetParent(RadarParent.RadarBG.transform);
    }


    private void MoveRadarBlip()
    {

        Vector3 TempPos = TrackedObject.transform.position - MyManager.PlayerTransform.position;

        TempPos.y = 0;

        float Temp = TempPos.z;
        TempPos.z = 0;
        TempPos.y = Temp;

        RadarTrackBlip.transform.localPosition = TempPos * RadarBlipRangeDelta;

        RadarTrackBlip.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void Update()
    {
        if (TrackedObject)
           TargetPosition = TrackedObject.transform.position;


        DistanceToTarget = Vector3.Distance(MyManager.PlayerTransform.position, TargetPosition);

        if (Vector3.Dot(Camera.main.gameObject.transform.forward, (TargetPosition - MyManager.PlayerTransform.position).normalized) >= 0)
        {
            HUDTracker.transform.position = Camera.main.WorldToScreenPoint(TargetPosition);
            Distance.text = (int)DistanceToTarget + "";

            if (!HUDWasOn)
            {
                HUDTracker.SetActive(true);
                HUDWasOn = HUDTracker.active;
            }
        }
        else
        {
            if (HUDWasOn)
            {
                HUDTracker.SetActive(false);
                HUDWasOn = HUDTracker.active;
            }
        }

        MoveRadarBlip();
    }
}
