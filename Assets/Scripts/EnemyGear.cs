using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGear : MonoBehaviour
{
    [SerializeField]
    protected BaseEnemy Controller;

    public bool Aimed = true;



    public virtual void AssignController(BaseEnemy a)
    {
        Controller = a;
        gameObject.layer = a.gameObject.layer;
    }

    public virtual void TriggerGear(bool Down)
    {

    }

    public virtual bool GearReady()
    {
        return false;
    }

    public virtual float GetBulletSpeed()
    {
        return 0;
    }
}
