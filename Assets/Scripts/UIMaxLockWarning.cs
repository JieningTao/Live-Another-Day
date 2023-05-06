using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaxLockWarning : MonoBehaviour
{
    [SerializeField]
    Animator MyAnimator;
    [SerializeField]
    Transform ParentObject;
    [SerializeField]
    AudioSource AlertPlayer;


    private void UpdateLockChanges(string Order, EnergySignal ES)
    {
        if (Order == "ClearLockRequester" || Order == "UnlockAll")
        {
            ParentObject.gameObject.SetActive(false);
        }
        else if (Order == "LockAtMax")
        {
            MyAnimator.SetTrigger("MaxLock");
        }
    }

    public void PlaySound()
    {
        AlertPlayer.Play();
    }

    private void OnEnable()
    {
        BaseMechFCS.LockChanges += UpdateLockChanges;
    }

    private void OnDisable()
    {
        BaseMechFCS.LockChanges -= UpdateLockChanges;
    }


}
