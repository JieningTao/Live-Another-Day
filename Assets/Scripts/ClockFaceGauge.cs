using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockFaceGauge : MonoBehaviour
{
    [SerializeField]
    Transform Clockface;
    [SerializeField]
    UnityEngine.UI.Text Indicator;

    void Update()
    {
        if (Clockface && Indicator)
        {
            int a = (int)Clockface.localEulerAngles.z;
            Indicator.text = a + "";
        }
        else
            Debug.LogError("ClockFaceGauge missing reference", this);

    }
}
