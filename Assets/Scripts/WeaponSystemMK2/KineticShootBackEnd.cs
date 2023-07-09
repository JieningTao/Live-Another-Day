using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KineticShootBackEnd : BaseMainEquipmentOption
{
    [SerializeField]
    public BaseShoot Weapon;
    [SerializeField]
    public string WeaponSN = "N-000";
    [SerializeField]
    public string WeaponName = "Null";
    [SerializeField]
    public Color WeaponGaugeColor;

    [Space(20)]

    [SerializeField]
    protected BaseShoot ShootScript;
    [SerializeField]
    protected int MaxMagazine;
    [SerializeField]
    protected int MaxReserveAmmo;
    [SerializeField]
    protected float ReloadTime;
    [Tooltip("Used to fetch additional amount from mech attribute, leave blank for weapons that do not gain additional ammo")]
    [SerializeField]
    protected string AmmoTypeIdentifier = "";
    protected int AttributeExtraAmmo;

    protected int MagazineRemaining;
    protected int ReserveRemaining;
    protected float ReloadTimeRemaining;

    protected virtual void Start()
    {
        MagazineRemaining = MaxMagazine;
        ReserveRemaining = MaxReserveAmmo + AttributeExtraAmmo;
    }

    protected virtual void Update()
    {
        if (ReloadTimeRemaining > 0)
            ReloadTimeRemaining -= Time.deltaTime;
    }

    //public override void Fire(bool Fire)
    //{
    //    if (MagazineRemaining > 0 && ReloadTimeRemaining <= 0)
    //    {
    //        MagazineRemaining--;
    //        base.Fire1();

    //        if (MagazineRemaining <= 0)
    //            Reload();
    //    }
    //    else
    //    {
    //        Reload();
    //    }
    //}

    //public void Reload()
    //{
    //    if (ReloadTimeRemaining > 0)
    //        return;

    //    if (ReserveRemaining > 0)
    //    {
    //        if (ReserveRemaining > MaxMagazine)
    //        {
    //            ReserveRemaining -= MaxMagazine;
    //            MagazineRemaining = MaxMagazine;
    //        }
    //        else
    //        {
    //            MagazineRemaining = ReserveRemaining;
    //            ReserveRemaining = 0;
    //        }
    //        ReloadTimeRemaining = ReloadTime;
    //        FireCooldown = 0;
    //    }

    //}

    #region UI Use
    public override Color GetInitBarColor
    { get { return WeaponGaugeColor; } }

    public override string GetInitFunctionText
    { get { return  WeaponSN + "\n" + WeaponName; } }

    public override float GetUpdateBarFill
    { get {
            if (ReloadTimeRemaining > 0)
                return 1 - (ReloadTimeRemaining / ReloadTime);
            else
                return (float)MagazineRemaining / (float)MaxMagazine;
        } }

    public override string GetUpdateText
    { get{
            if (ReloadTimeRemaining > 0)
                return "Reloading";
            else
                return MagazineRemaining + "/" + ReserveRemaining; ;
        } }
    #endregion
    public virtual string GetMag
    { get { return MaxMagazine + "/" + (MaxReserveAmmo + AttributeExtraAmmo); } }

    public virtual string GetReload
    { get { return ReloadTime + "s"; } }

    public string GetAmmoIdentifier
    { get { return AmmoTypeIdentifier; } }
}
