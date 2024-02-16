using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

public class BaseMechInteractable : MonoBehaviour
{
    [SerializeField]
    protected string Name;
    [SerializeField]
    protected string MainInteract;
    [SerializeField]
    protected string SubInteract;
    [SerializeField]
    protected bool IsInteractable = true;

    public virtual void InteractMain(BaseMechMain Mech, bool a)
    {

    }

    public virtual void InteractSub(BaseMechMain Mech, bool a)
    {

    }

    public virtual string InteractableName
    { get { return Name; } }
    public virtual string MainInteractName
    { get { return MainInteract; } }
    public virtual string SubInteractName
    { get { return SubInteract; } }
    public virtual bool Interactable
    { get { return IsInteractable; } }
}
