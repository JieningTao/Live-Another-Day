using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSchemeChanger : MonoBehaviour
{

    [SerializeField]
    private UnityEngine.UI.Text MaterialTitle;
    [SerializeField]
    private Material Main;
    [SerializeField]
    private Material Secondary;
    [SerializeField]
    private Material Frame;

    private Material CurrentAdjustingMaterial;
    [Space(20)]
    [SerializeField]
    private UnityEngine.UI.Slider ColorHSlider;
    [SerializeField]
    private UnityEngine.UI.Slider ColorSSlider;
    [SerializeField]
    private UnityEngine.UI.Slider ColorVSlider;
    [SerializeField]
    private UnityEngine.UI.Slider MetallicSlider;
    [SerializeField]
    private UnityEngine.UI.Slider SmoothSlider;

    [Space(20)]
    [SerializeField]
    private UnityEngine.UI.Image MainMatButton;
    [SerializeField]
    private UnityEngine.UI.Image SecondaryMatButton;
    [SerializeField]
    private UnityEngine.UI.Image FrameMatButton;

    private float H, S, V;

    private void Start()
    {
        MainMatButton.color = Main.color;
        SecondaryMatButton.color = Secondary.color;
        FrameMatButton.color = Frame.color;
    }
    #region Button Functions
    public void SetCurrentMain()
    {
        MaterialTitle.text = "Main Material";
        CurrentAdjustingMaterial = Main;
        LoadMaterial();
    }

    public void SetCurrentSecondary()
    {
        MaterialTitle.text = "Secondary Material";
        CurrentAdjustingMaterial = Secondary;
        LoadMaterial();
    }

    public void SetCurrentFrame()
    {
        MaterialTitle.text = "Frame Material";
        CurrentAdjustingMaterial = Frame;
        LoadMaterial();
    }
    #endregion

    #region Slider Functions
    public void SetColorH(System.Single _H)
    {
        H = _H;
        CurrentAdjustingMaterial.color = Color.HSVToRGB(H, S, V);

        if(CurrentAdjustingMaterial == Main)
            MainMatButton.color = Main.color;
        else if(CurrentAdjustingMaterial == Secondary)
            SecondaryMatButton.color = Secondary.color;
        else
            FrameMatButton.color = Frame.color;

    }
    public void SetColorS(System.Single _S)
    {
        S = _S;
        CurrentAdjustingMaterial.color = Color.HSVToRGB(H, S, V);

        if (CurrentAdjustingMaterial == Main)
            MainMatButton.color = Main.color;
        else if (CurrentAdjustingMaterial == Secondary)
            SecondaryMatButton.color = Secondary.color;
        else
            FrameMatButton.color = Frame.color;
    }
    public void SetColorV(System.Single _V)
    {
        V = _V;
        CurrentAdjustingMaterial.color = Color.HSVToRGB(H, S, V);

        if (CurrentAdjustingMaterial == Main)
            MainMatButton.color = Main.color;
        else if (CurrentAdjustingMaterial == Secondary)
            SecondaryMatButton.color = Secondary.color;
        else
            FrameMatButton.color = Frame.color;
    }

    public void SetMetallic(System.Single Mt)
    {
        CurrentAdjustingMaterial.SetFloat("_Metallic", Mt);
    }
    public void SetSmooth(System.Single Sm)
    {
        CurrentAdjustingMaterial.SetFloat("_Glossiness", Sm);
    }
    #endregion

    private void LoadMaterial()
    {
        Color.RGBToHSV(CurrentAdjustingMaterial.color, out H, out S, out V);

        Debug.Log(H + "-" + S + "-" + V);
        ColorHSlider.value = H;
        ColorSSlider.value = S;
        ColorVSlider.value = V;

        MetallicSlider.value = CurrentAdjustingMaterial.GetFloat("_Metallic");
        SmoothSlider.value = CurrentAdjustingMaterial.GetFloat("_Glossiness");

    }

}
