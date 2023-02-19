using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SceneEventHandler 
{
    public static event Action<String,object> SceneEventSignal;




    public static void invokeSES(string Identifier,object Content)
    {
        if (SceneEventSignal != null)
            SceneEventSignal.Invoke(Identifier, Content);
        else
            Debug.Log("No Listeners for SES");
    }
}
