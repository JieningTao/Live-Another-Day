using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RemoteWeaponaryEvents : MonoBehaviour
{
    public static event Action<UnityEngine.Object,String, object> RemoteWeaponSignal;

    public static void InvokeRWS(UnityEngine.Object Source, string Identifier, object Content)
    {
        if (RemoteWeaponSignal != null)
            RemoteWeaponSignal.Invoke(Source, Identifier, Content);
        else
            Debug.Log("No Listeners for RWS");
    }

}
