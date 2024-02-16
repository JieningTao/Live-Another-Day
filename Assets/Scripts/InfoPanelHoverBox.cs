using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelHoverBox : MonoBehaviour
{
    private static InfoPanelHoverBox _Instance;
    public static InfoPanelHoverBox Instance
    {
        get
        {
            if (!_Instance)
                _Instance = FindObjectOfType<InfoPanelHoverBox>();
            return _Instance;
        }
    }

    [SerializeField]
    List<DisplayLane> Displays;
    [SerializeField]
    List<InfoDetail> InfoDetails;
    [SerializeField]
    GameObject PanelObject;


    [Serializable]
    public class DisplayLane
    {
        [SerializeField]
        public GameObject GO;
        [SerializeField]
        public Image Image;
        [SerializeField]
        public Text Text;
    }

    [Serializable]
    public class InfoDetail
    {
        [SerializeField]
        public Sprite InfoSprite;
        [SerializeField]
        public string Detail;
    }

    public void GetCalled(List<Sprite> InfoSprites,Vector3 Loc)
    {
        PanelObject.SetActive(true);
        NewPanel(InfoSprites);
        transform.position = Loc;
    }

    public void NewPanel(List<Sprite> InfoSprites)
    {
        for (int i = 0; i < 4; i++)
        {
            if (InfoSprites[i] != null)
            {
                Displays[i].GO.SetActive(true);
                Displays[i].Image.sprite = InfoSprites[i];
                Displays[i].Text.text = FetchDetail(InfoSprites[i]);
            }
            else
            {
                Displays[i].GO.SetActive(false);
            }

        }
    }
    public void HidePanel()
    {
        PanelObject.SetActive(false);
    }

    private string FetchDetail(Sprite a)
    {
        foreach (InfoDetail b in InfoDetails)
        {
            if (b.InfoSprite == a)
                return b.Detail;
        }
        return "";
    }
}
