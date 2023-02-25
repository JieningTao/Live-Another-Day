using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractUIManager : MonoBehaviour
{
    [SerializeField]
    NamePlatedItem ObjectName;

    [SerializeField]
    NamePlatedItem RightPrimary;
    [SerializeField]
    NamePlatedItem RightSecondary;
    [SerializeField]
    GameObject RightHoldGauge;

    [Space(20)]

    [SerializeField]
    NamePlatedItem LeftPrimary;
    [SerializeField]
    NamePlatedItem LeftSecondary;
    [SerializeField]
    GameObject LeftHoldGauge;
    [Serializable]

    public class NamePlatedItem
    {
        public GameObject NamePlate;
        public UnityEngine.UI.Text Text;

        public void SetText(string _Text)
        {
            Text.text = _Text;
            if (Text.text == "")
                NamePlate.SetActive(false);
            else
                NamePlate.SetActive(true);
        }
    }

    void NewObject(string Object,string RP, string RS, string LP, string LS)
    {
        ObjectName.SetText(Object);
        RightPrimary.SetText(RP);
        RightSecondary.SetText(RS);
        LeftPrimary.SetText(LP);
        LeftSecondary.SetText(LS);
    }

    void HoldGauge(bool Right, bool Hold)
    {
        if (Right)
            RightHoldGauge.SetActive(Hold);
        else
            LeftHoldGauge.SetActive(Hold);
    }
}
