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
    public bool RequireAiming = true;

    [SerializeField]
    private float Weight = 0.1f;
        




    public virtual void PrimaryFire(bool Fire)
    {

    }

    public virtual void SecondaryFire(bool Fire)
    {

    }

    public virtual void Equip(bool _Equip, BaseMechMain Operator )
    {
        this.gameObject.layer = Operator.gameObject.layer;
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
}
