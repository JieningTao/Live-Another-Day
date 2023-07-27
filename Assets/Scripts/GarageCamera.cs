using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GarageCamera : MonoBehaviour, IPointerDownHandler
{

    [SerializeField]
    GameObject HorizontalAnchor;
    [SerializeField]
    GameObject VerticalAnchor;
    [SerializeField]
    float TurnSpeed = 10;
    [Space(20)]
    [SerializeField]
    GameObject SPHorizontalAnchor;
    [SerializeField]
    GameObject SPVerticalAnchor;

    bool IsDragging;
    float DragTime;



    private void Update()
    {
        if (IsDragging)
        {
            DragTime += Time.deltaTime;
            if (DragTime > 0.2f)
            {
                HorizontalAnchor.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * TurnSpeed);
                VerticalAnchor.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * -1, 0, 0) * Time.deltaTime * TurnSpeed);

                if(SPHorizontalAnchor)
                    SPHorizontalAnchor.transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * TurnSpeed);
                if (SPVerticalAnchor)
                    SPVerticalAnchor.transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * -1, 0, 0) * Time.deltaTime * TurnSpeed);
            }
            if (Input.GetMouseButtonUp(0))
                EndDrag();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            IsDragging = true;
            Debug.Log(this.gameObject.name + " Was Left Clicked.");
        }

    }

    private void EndDrag()
    {
        IsDragging = false;

        if (DragTime < 0.2f)
        {
            HorizontalAnchor.transform.rotation = Quaternion.Euler(Vector3.zero);
            VerticalAnchor.transform.rotation = Quaternion.Euler(Vector3.zero);

            if (SPHorizontalAnchor)
                SPHorizontalAnchor.transform.rotation = Quaternion.Euler(Vector3.zero);
            if (SPVerticalAnchor)
                SPVerticalAnchor.transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        DragTime = 0;
    }


}
