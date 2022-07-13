using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGear : MonoBehaviour
{
    [SerializeField]
    protected BaseEnemy Controller;

    public bool Aimed = true;

    public void AssignController(BaseEnemy a)
    {
        Controller = a;
    }

    public virtual void TriggerGear(bool Down)
    {

    }

    public virtual bool GearReady()
    {
        return false;
    }
}
