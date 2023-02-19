using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMainSlotEquipment : MonoBehaviour
{
    [SerializeField]
    public MainEquipmentHoldStyle HoldStyle;

    public enum MainEquipmentHoldStyle
    {
        Empty = 0,
        DefaultWeapon = 1,
        ShoulderWeapon = 2,
        Shield = 3,
        UnderWeapon = 4,
    }

    [SerializeField]
    public MainEquipmentSize Size = MainEquipmentSize.Medium;

    public enum MainEquipmentSize
    {
        Special,

        ExtraSmall,
        Small,
        Medium,
        Large,
        ExtraLarge,
    }

    [SerializeField]
    public bool RequireAiming = true;

    [SerializeField]
    private float Weight = 0.1f;

    [SerializeField]
    private bool Mirrored = false;

    protected bool Right;
    protected BaseMechFCS Operator;



    public virtual void PrimaryFire(bool Fire)
    {

    }

    public virtual void SecondaryFire(bool Fire)
    {

    }

    public virtual void Equip(bool _Equip, BaseMechMain _Operator,bool _Right)
    {
        if (_Operator)
        {
            Operator = _Operator.GetFCS();
            this.gameObject.layer = _Operator.gameObject.layer;
        }

        Right = _Right;

        if (!Right)
        {
            if (Mirrored)
            {
                Vector3 a = transform.localScale;
                a.x = -Mathf.Abs(transform.localScale.x);

                transform.localScale = a;
            }
        }
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

    public virtual float GetBulletSpeed()
    {
        return 0;
    }

    public virtual float GetWeight()
    {
        return Weight;
    }

    public virtual List<string> GetStats()
    {
        return null;
    }
}
