using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfoSquareHoverDetector : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    AssemblyPartOption APOParent;


    public void OnPointerEnter(PointerEventData eventData)
    {
        InfoPanelHoverBox.Instance.GetCalled(APOParent.ExtraceInfoSprites(), transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InfoPanelHoverBox.Instance.HidePanel();
    }
}
