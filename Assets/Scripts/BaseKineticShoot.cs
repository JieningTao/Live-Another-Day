using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseKineticShoot : BaseShoot
{
    [Space(20)]
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


    protected override void Start()
    {
        MagazineRemaining = MaxMagazine;
        ReserveRemaining = MaxReserveAmmo + AttributeExtraAmmo;
        base.Start();
    }

    protected override void Update()
    {
        if (ReloadTimeRemaining > 0)
            ReloadTimeRemaining -= Time.deltaTime;
        else
            base.Update();

    }

    public void SetAttributeExtraAmmo(float ExtraPercent)
    {
        AttributeExtraAmmo = (int)(ExtraPercent * (float)MaxReserveAmmo);
        ReserveRemaining = MaxReserveAmmo + AttributeExtraAmmo;

        Debug.Log(MaxReserveAmmo + "-->" + (MaxReserveAmmo + AttributeExtraAmmo));
        Debug.Log(ReserveRemaining);
    }

    public override void EquipWeapon()
    {
        base.EquipWeapon();


    }

    public void Reload()
    {
        if (ReloadTimeRemaining > 0)
            return;

        if (ReserveRemaining > 0)
        {
            if (ReserveRemaining > MaxMagazine)
            {
                ReserveRemaining -= MaxMagazine;
                MagazineRemaining = MaxMagazine;
            }
            else
            {
                MagazineRemaining = ReserveRemaining;
                ReserveRemaining = 0;
            }
            ReloadTimeRemaining = ReloadTime;
            FireCooldown = 0;
        }
        Debug.Log(ReserveRemaining);

    }

    public void ReArm(float Percent)
    {
        Percent = Mathf.Clamp(Percent, 0, 1);
        ReserveRemaining += (int)(Percent * (MaxReserveAmmo + AttributeExtraAmmo));
        ReserveRemaining = Mathf.Clamp(ReserveRemaining, 0, (MaxReserveAmmo + AttributeExtraAmmo));
    }

    protected override void Fire1()
    {
        if (MagazineRemaining > 0&&ReloadTimeRemaining<=0)
        {
            MagazineRemaining--;
            base.Fire1();

            if (MagazineRemaining <= 0)
                Reload();
        }
        else
        {
            Reload();
        }
    }

    public override float GetAmmoGauge()
    {
        if (ReloadTimeRemaining > 0)
            return 1-( ReloadTimeRemaining / ReloadTime);
        else
            return (float)MagazineRemaining / (float)MaxMagazine;
    }

    public override string GetAmmoText()
    {
        if (ReloadTimeRemaining > 0)
            return "Reloading";
        else
            return MagazineRemaining+ "/"+ ReserveRemaining;
    }

    public override bool GetFirable()
    {
        return ReloadTimeRemaining<=0;
    }

    public override bool LowAmmoWarning()
    {
        if (ReserveRemaining <= MaxMagazine)
            return true;

        return false;
    }

    public override string GetMag
    { get { return MaxMagazine+"/"+(MaxReserveAmmo+AttributeExtraAmmo); } }

    public override string GetReload
    { get { return ReloadTime+"s"; } }

    public string GetAmmoIdentifier
    { get { return AmmoTypeIdentifier; } }
}
