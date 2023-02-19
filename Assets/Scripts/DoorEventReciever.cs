using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEventReciever : MonoBehaviour
{
    [SerializeField]
    string Identifier;
    [SerializeField]
    Animator DoorAnimator;





    private void SignalRecieved(string a, object b)
    {
        if (a == Identifier)
        {
            DoorAnimator.SetBool("Open", (bool)b);
        }
    }


    private void OnEnable()
    {
        SceneEventHandler.SceneEventSignal += SignalRecieved;
    }

    private void OnDisable()
    {
        SceneEventHandler.SceneEventSignal -= SignalRecieved;
    }

}
