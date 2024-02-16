using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerInteractable : BaseMechInteractable
{

    public UnityEvent MainInteractDown;
    public UnityEvent MainInteractUp;
    public UnityEvent SubInteractDown;
    public UnityEvent SubInteractUp;

    public override void InteractMain(BaseMechMain Mech, bool a)
    {
        if (a)
            MainInteractDown.Invoke();
        else
            MainInteractUp.Invoke(); ;
    }

    public override void InteractSub(BaseMechMain Mech, bool a)
    {
        if (a)
            SubInteractDown.Invoke();
        else
            SubInteractUp.Invoke();
    }
}
