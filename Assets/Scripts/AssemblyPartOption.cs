using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssemblyPartOption : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text NameText;
    [SerializeField]
    UnityEngine.UI.Text ClassificationText;
    [SerializeField]
    LoadOutPart Part;

    [SerializeField]
    GameObject InfoSpriteParent;
    [SerializeField]
    UnityEngine.UI.Image SquareTL;
    [SerializeField]
    UnityEngine.UI.Image SquareTR;
    [SerializeField]
    UnityEngine.UI.Image SquareBL;
    [SerializeField]
    UnityEngine.UI.Image SquareBR;

    private PartSwitchManager MyManager;
    private bool selected = false;

   


    public void SetUp(PartSwitchManager Manager,LoadOutPart _Part)
    {
        MyManager = Manager;

        if (_Part == null)
        {
            Part = null;
            InfoSpriteParent.SetActive(false);
            NameText.text = "Empty";
            ClassificationText.text = "Unequip Current";
        }
        else
        {
            Part = _Part;
            NameText.text = Part.Name;
            ClassificationText.text = Part.Classification;
            InfoSpriteParent.SetActive(true);
            AssignInfoSprites();
        }
    }

    private void AssignInfoSprites()
    {
        Sprite[] Temp = Part.InfoSprites;

        if (Temp[0])
            SquareTL.sprite = Temp[0];
        if (Temp[1])
            SquareTR.sprite = Temp[1];
        if (Temp[2])
            SquareBL.sprite = Temp[2];
        if (Temp[3])
            SquareBR.sprite = Temp[3];

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
        if (Part)
        {
            Debug.Log(Part.gameObject.name);
            MyManager.ButtonClickedPart(Part);
        }
        else
        {
            Debug.Log("[Empty]");
            MyManager.ButtonClickedPart(null);
        }
    }

    public List<Sprite> ExtraceInfoSprites()
    {
        List<Sprite> Temp = new List<Sprite>();
        Temp.AddRange(Part.InfoSprites);
        return Temp;
    }
}
