using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyPartOption : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text NameText;
    [SerializeField]
    LoadOutPart Part;

    private PartSwitchManager MyManager;
    private bool selected = false;

   


    public void SetUp(PartSwitchManager Manager,LoadOutPart _Part)
    {
        if (_Part == null)
            gameObject.SetActive(false);
        else
        {
            MyManager = Manager;
            Part = _Part;
            NameText.text = Part.Name;
        }
    }

    public void SetUp( LoadOutPart _Part)
    {

        if (_Part == null)
            gameObject.SetActive(false);
        else
        {
            Part = _Part;
            NameText.text = Part.Name;
        }
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
        Debug.Log(Part.gameObject.name);
        MyManager.InstallPart(Part.gameObject);
    }

}
