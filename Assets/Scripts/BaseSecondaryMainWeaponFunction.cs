using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSecondaryMainWeaponFunction : MonoBehaviour
{
    public virtual void InitFunction(BaseMechMain Operator)
    {

    }

    public virtual void Trigger(bool Down)
    {

    }

    public virtual bool FunctionReady
    { get { return true; } }

    public virtual string UpdateText
    { get { return ""; } }
}
