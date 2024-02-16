using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MechCompareStat : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Text Old;
    [SerializeField]
    private UnityEngine.UI.Text New;
    [SerializeField]
    private UnityEngine.UI.Image Pointer;
    [SerializeField]
    private string Unit;
    [SerializeField]
    private Sprite ValueChangeSprite;
    [SerializeField]
    private Sprite DifferentSprite;
    [SerializeField]
    private Color UpColor;
    [SerializeField]
    private Color MidColor;
    [SerializeField]
    private Color DownColor;




    private void PointerUp()
    {
        Pointer.sprite = ValueChangeSprite;
        Pointer.transform.rotation = Quaternion.Euler(0, 0, 90);
        Pointer.color = UpColor;
    }

    private void PointerDown()
    {
        Pointer.sprite = ValueChangeSprite;
        Pointer.transform.rotation = Quaternion.Euler(0, 0, -90);
        Pointer.color = DownColor;
    }

    private void PointerChanged()
    {
        Pointer.sprite = DifferentSprite;
        Pointer.transform.rotation = Quaternion.Euler(0, 0, 0);
        Pointer.color = MidColor;
    }

    public void RecieveValue(object a)
    {

    }


}
