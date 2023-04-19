using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMechPart : MonoBehaviour
{
    public float Health = 1;
    public float Weight = 0.1f;

    [SerializeField]
    public List<AttributeManager.BaseMechAttribute> Attributes = new List<AttributeManager.BaseMechAttribute>();
    

    public List<Transform> FloatThrusters;
    public List<Transform> BoostThrusters;

    [SerializeField]
    protected Vector3 Displacement;

    [SerializeField]
    protected Transform JointPosition;



    protected BaseMechMain MyMech;


    public virtual void Assemble(BaseMechMain Mech, Transform SocketPosition)
    {
        VisualAssemble(SocketPosition);
        MyMech = Mech;
        SetLayer(Mech.gameObject.layer);
        Mech.ApplyMechAttributes(Attributes);
    }

    public virtual void VisualAssemble(Transform SocketPosition)
    {
        transform.parent = SocketPosition;
        //transform.localPosition = Displacement;
        Vector3 a = new Vector3(JointPosition.localPosition.x * transform.localScale.x, JointPosition.localPosition.y * transform.localScale.y, JointPosition.localPosition.z * transform.localScale.z) * - 1;
        //Debug.Log("Scale: "+ transform.localScale+"\nLP: "+JointPosition.localPosition+"\nDisplacement: "+a, this);
        transform.localPosition = a;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public virtual void SetLayer(int Layer)
    {
        foreach (Collider a in GetComponentsInChildren<Collider>())
        {
            a.gameObject.layer = Layer;
        }
    }

    public virtual float GetWeight(bool IncludeGear)
    {
        return Weight;
    }

    #region Loadoutpart request info stuff
    public virtual string GetHealth
    { get { return Health+""; } }

    public virtual string GetPartWeight
    { get { return Weight+""; } }

    public virtual string GetBIEXG
    { get { return null; } }

    public virtual string GetEXGSlots
    { get { return null; } }
    #endregion

}
