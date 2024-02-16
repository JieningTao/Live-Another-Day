using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableUIManager : MonoBehaviour
{
    [SerializeField]
    GameObject InteractableDisplayMain;
    [SerializeField]
    Text InteractableName;
    [SerializeField]
    GameObject InteractMainOption;
    [SerializeField]
    Text InteractMainOpptionText;
    [SerializeField]
    GameObject InteractSubOption;
    [SerializeField]
    Text InteractSubOpptionText;

    BaseMechInteractable CurrentInteractable;

    public void UpdateDisplay(BaseMechInteractable a)
    {
        CurrentInteractable = a;

        if (a)
            InteractableDisplayMain.SetActive(true);
        else
        {
            InteractableDisplayMain.SetActive(false);
            return;
        }

        InteractableName.text = a.InteractableName;

        if (a.MainInteractName != null && a.MainInteractName != "")
        {
            InteractMainOption.SetActive(true);
            InteractMainOpptionText.text = a.MainInteractName;
        }
        else
            InteractMainOption.SetActive(false);

        if (a.SubInteractName != null && a.SubInteractName != "")
        {
            InteractSubOption.SetActive(true);
            InteractSubOpptionText.text = a.SubInteractName;
        }
        else
            InteractSubOption.SetActive(false);

    }
}
