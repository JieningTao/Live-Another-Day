using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEXGear : MonoBehaviour
{
    [SerializeField]
    protected BaseMechFCS MyFCS;


    // Start is called before the first frame update
    void Start()
    {

    }

    public virtual void InitializeGear(BaseMechFCS FCS)
    {
        MyFCS = FCS;
    }

    public virtual void TriggerGear(bool Down)
    {
        
    }

}
