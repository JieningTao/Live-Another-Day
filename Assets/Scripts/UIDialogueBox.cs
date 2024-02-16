using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDialogueBox : MonoBehaviour
{

    [SerializeField]
    UnityEngine.UI.Image LeftEmblem;
    [SerializeField]
    UnityEngine.UI.Text LeftName;
    [SerializeField]
    UnityEngine.UI.Image LeftCover;
    [Space(20)]
    [SerializeField]
    UnityEngine.UI.Image RightEmblem;
    [SerializeField]
    UnityEngine.UI.Text RightName;
    [SerializeField]
    UnityEngine.UI.Image RightCover;
    [Space(20)]
    [SerializeField]
    UnityEngine.UI.Text TextBox;
    [SerializeField]
    GameObject ContentMaster;
    [Space(20)]
    [SerializeField]
    float LingerTime; //time the text box lingers after diaplay time has past
    float LingerTimeRemaining;
    float Displaytime; //time the current text will stay for before starting to disappear, based upon how long the text displayed is
    [SerializeField]
    Sprite UnknownContact;






    private void Update()
    {
        CheckHide();
    }

    private void CheckHide()
    {
        if (ContentMaster.active)
        {
            if(Displaytime>0)
                Displaytime -= Time.deltaTime;
            else if(LingerTimeRemaining>0)
            {
                LingerTimeRemaining -= Time.deltaTime;
                if (LingerTimeRemaining < 0)
                    ContentMaster.SetActive(false);
            }
        }
    }

    private void NewText(Sprite Speaker,string Name,string Speech,float _DisplayTime)
    {
        ContentMaster.SetActive(true);

        if (Speaker)
            LeftEmblem.sprite = Speaker;
        else
            LeftEmblem.sprite = UnknownContact;

        LeftName.text = Name;
        TextBox.text = Speech;
        LingerTimeRemaining = LingerTime;
        Displaytime = _DisplayTime;
    }

}
