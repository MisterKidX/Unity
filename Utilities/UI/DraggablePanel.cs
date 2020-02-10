using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DraggablePanel : MonoBehaviour, IDragHandler, IPointerDownHandler
{

    #region Properties

    Vector2 mouseOffset;
    Vector3 newPosition;
    RectTransform parent;

    #endregion

    #region Unity Locals

    void Awake () 
	{
        parent = transform.parent as RectTransform;
	}

	void Update () 
	{
		
	}

    #endregion

    #region Methods

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out newPosition);
        parent.position = newPosition - (Vector3)mouseOffset;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        parent.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform.parent, eventData.position, eventData.pressEventCamera, out mouseOffset);
    }

    #endregion
}
