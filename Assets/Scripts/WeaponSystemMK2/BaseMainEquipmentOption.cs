using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMainEquipmentOption : MonoBehaviour
{


    public virtual void Fire(bool Fire)
    {

    }

    public virtual Color GetInitBarColor
    { get { return Color.black; } }

    public virtual string GetInitFunctionText
    { get { return ""; } }

    public virtual float GetUpdateBarFill
    { get { return 0; } }

    public virtual string GetUpdateText
    { get { return ""; } }
}
