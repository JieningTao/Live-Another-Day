using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyPartOption : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text NameText;
    [SerializeField]
    BaseMechPart Part;

    private PartSwitchManager MyManager;
    private bool selected = false;

   


    public void SetUp(PartSwitchManager Manager)
    {
        MyManager = Manager;
    }

    public void Select()
    {
        if (selected)
            Confirm();
        else
            selected = true;
    }

    public void Confirm()
    {
        MyManager.InstallPart(Part.gameObject);
    }

}
