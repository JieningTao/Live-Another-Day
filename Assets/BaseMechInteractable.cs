using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class BaseMechInteractable : MonoBehaviour
{
    [SerializeField]
    private string Name;
    [SerializeField]
    private string MainInteract;
    [SerializeField]
    private string SubInteract;

    public UnityEvent MainInteractDown; 
    public UnityEvent MainInteractUp;
    public UnityEvent SubInteractDown;
    public UnityEvent SubInteractUp;

    public void InteractMain(bool a)
    {
        if (a)
            MainInteractDown.Invoke();
        else
            MainInteractUp.Invoke(); ;
    }

    public void InteractSub(bool a)
    {
        if (a)
            SubInteractDown.Invoke();
        else
            SubInteractUp.Invoke();
    }

    public virtual string InteractableName
    {get { return Name; }}
    public virtual string MainInteractName
    { get { return MainInteract; } }
    public virtual string SubInteractName
    { get { return SubInteract; } }
}
