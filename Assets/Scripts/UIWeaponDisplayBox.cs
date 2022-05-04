using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWeaponDisplayBox : MonoBehaviour
{
    [SerializeField]
    UnityEngine.UI.Text Name;
    [SerializeField]
    UnityEngine.UI.Text Ammo;
    [SerializeField]
    UnityEngine.UI.Image Fillbar;
    [SerializeField]
    GameObject ContentParent;
    [SerializeField]
    UnityEngine.UI.Image BG;
    [SerializeField]
    Color BGAvaliableColor;
    [SerializeField]
    Color BGUnavaliableColor;




    public void StartInitialize(string WeaponName,Color BarColor)
    {
        Name.text = WeaponName;
        Fillbar.color = BarColor;
    }

    public void RegularUpdate(string _Ammo,float FillPercentage)
    {
        Ammo.text = _Ammo;
        Fillbar.fillAmount = FillPercentage;
    }

    public void SetAvaliable(bool Avaliable)
    {
        ContentParent.SetActive(Avaliable);

        if (Avaliable)
            BG.color = BGAvaliableColor;
        else
            BG.color = BGUnavaliableColor;
    }

}
