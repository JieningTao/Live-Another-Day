using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechInteract : MonoBehaviour
{
    [SerializeField]
    BaseMechMain MyMech;
    InteractableUIManager UIManager;
    [SerializeField]//SFT
    List<BaseMechInteractable> InteractablesInRange = new List<BaseMechInteractable>();
    [SerializeField]//SFT
    BaseMechInteractable CurrentInteractable;

    private void Start()
    {
        UIManager = FindObjectOfType<InteractableUIManager>();
    }

    private void Update()
    {
        if (InteractablesInRange.Count > 1)
            FindMainTarget();
        else if(InteractablesInRange.Count ==1)
        {
            if (!CurrentInteractable.gameObject.active)
            {
                InteractablesInRange.Remove(CurrentInteractable);
                CurrentInteractable = null;
                UIManager.UpdateDisplay(null);
            }
        }
        HandleInput();
    }

    private void NewMainInteractable(BaseMechInteractable a)
    {
        CurrentInteractable = a;
        UIManager.UpdateDisplay(CurrentInteractable);
    }

    private void FindMainTarget()
    {
        if (InteractablesInRange.Count <= 1)
        {
            return;
        }
        else
        {
            BaseMechInteractable OldTarget = CurrentInteractable;
            float Dis = 100; //default dis is large so the first target will auto replace it as closest to the interactable zone

            for (int i = 0; i < InteractablesInRange.Count; i++)
            {
                if (!InteractablesInRange[i].gameObject.active)
                {
                    InteractablesInRange.RemoveAt(i);
                    i--;
                }
                else
                {
                    float NewDis = Vector3.Distance(InteractablesInRange[i].transform.position, transform.position);

                    if (NewDis < Dis)
                    {
                        CurrentInteractable = InteractablesInRange[i];
                        Dis = NewDis;
                    }
                }
            }

            if (OldTarget != CurrentInteractable)
            {
                NewMainInteractable(CurrentInteractable);
            }

        }
    }

    private void HandleInput()
    {
        if (CurrentInteractable != null && Time.timeScale != 0)
        {
            if (Input.GetButtonDown("InteractMain"))
                CurrentInteractable.InteractMain(MyMech,true);
             else if(Input.GetButtonUp("InteractMain"))
                CurrentInteractable.InteractMain(MyMech, false);

            if (Input.GetButtonDown("InteractSub"))
                CurrentInteractable.InteractSub(MyMech, true);
            else if (Input.GetButtonUp("InteractSub"))
                CurrentInteractable.InteractSub(MyMech, false);
        }


    }
    private void OnTriggerEnter(Collider other)
    {
        InteractablesInRange.Add(other.GetComponentInParent<BaseMechInteractable>());
        if (InteractablesInRange.Count == 1)
           NewMainInteractable( InteractablesInRange[0]);
    }

    private void OnTriggerExit(Collider other)
    {
        BaseMechInteractable Temp = other.GetComponentInParent<BaseMechInteractable>();
        InteractablesInRange.Remove(Temp);

        if (InteractablesInRange.Count == 0)
            NewMainInteractable(null);
        else if (InteractablesInRange.Count == 1)
            NewMainInteractable(InteractablesInRange[0]);
    }


}
