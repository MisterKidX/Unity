using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ResizeablePanel : MonoBehaviour, IPointerDownHandler, IDragHandler
{

    #region Properties

    RectTransform _parent;
    Vector2 _currentPointerPosition;
    Vector2 _lastPointerPosition;


    #endregion

    #region Unity Locals

    void Awake () 
	{
        _parent = transform.parent as RectTransform;
	}

	void Update () 
	{
		
	}

    #endregion

    #region Methods

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_parent, eventData.position, eventData.pressEventCamera, out _currentPointerPosition);
        var currentSize = _parent.sizeDelta;

        _parent.sizeDelta += new Vector2(_currentPointerPosition.x - _lastPointerPosition.x, -_currentPointerPosition.y + _lastPointerPosition.y);

        _lastPointerPosition = _currentPointerPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _parent.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_parent, eventData.position, eventData.pressEventCamera, out _lastPointerPosition);
    }

    #endregion
}
