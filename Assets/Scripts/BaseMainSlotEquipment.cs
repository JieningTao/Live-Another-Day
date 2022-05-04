using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMainSlotEquipment : MonoBehaviour
{





    public virtual void PrimaryFire(bool Fire)
    {

    }

    public virtual void SecondaryFire(bool Fire)
    {

    }

    public virtual void GetInitializeDate(out string MainFunction,out Color MainColor, out string SecondaryFunction,out Color SecondaryColor)
    {
        MainFunction = "";
        MainColor = Color.black;
        SecondaryFunction = "";
        SecondaryColor = Color.black;
    }

    public virtual void GetUpdateData(bool Main, out float BarFillPercentage, out string TextDisplay)
    {
        BarFillPercentage = 0;
        TextDisplay = "";
    }
}
