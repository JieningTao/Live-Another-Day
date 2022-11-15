using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseShield : IDamageable
{

    #region InfoAccess

    public string GetMaxHealth
    { get { return MaxHealth + ""; } }

    #endregion
}
