using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadarUI : MonoBehaviour
{
    [SerializeField]
    public RectTransform RadarBG;
    [SerializeField]
    UILockManager UILM;
    [SerializeField]
    UnityEngine.UI.Text MaxRangeDisplay;
    [SerializeField]
    RectTransform LockRangeDisplay;

    private float RangeDelta;


    public void SetRanges(float RadarRange,float LockRange)
    {
        //Debug.Log(RadarRange+"  "+LockRange);
        MaxRangeDisplay.text = (int)RadarRange + "";

        RangeDelta = (RadarBG.sizeDelta.x / RadarRange)/2;
        //Debug.Log(RangeDelta);
        float Temp = LockRange / RadarRange * RadarBG.sizeDelta.x;
        LockRangeDisplay.sizeDelta = new Vector2(Temp,Temp);

    }

    public float GetRangeDelta()
    {
        return RangeDelta;
    }

    private void Update()
    {

        //Debug.Log(UILM.PlayerTransform.eulerAngles.y);
        RadarBG.rotation = Quaternion.Euler(0, 0, UILM.PlayerTransform.eulerAngles.y);
        /*
        for (int i = 0; i < UILM.LockedEnemies.Count; i++)
        {
            if (i  >= ManagedBlips.Count)
            {
                GameObject a = Instantiate(EnemyBlipPrefab,RadarBG);
                ManagedBlips.Add(a);
            }

            Vector3 TempPos = UILM.LockedEnemies[i].position - UILM.PlayerTransform.position;

            TempPos.y = 0;

            float Temp = TempPos.z;
            TempPos.z = 0;
            TempPos.y = Temp;

            ManagedBlips[i].transform.localPosition = TempPos;



        }
        */
    }






}
