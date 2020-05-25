using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePanel : MonoBehaviour, IDragHandler, IPointerDownHandler
{
    #region Properties

    Vector2 mouseOffset;
    Vector3 newPosition;
    RectTransform parent;

    public float leftClamp = 0;
    public float rightClamp = 0;
    public float topClamp = 0;
    public float bottomClamp = 0;

    #endregion

    #region Unity Locals

    void Awake()
    {
        parent = transform.parent as RectTransform;
    }

    #endregion

    #region Methods

    public void OnDrag(PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(transform.parent as RectTransform, eventData.position, eventData.pressEventCamera, out newPosition);

        if ((leftClamp == 0 || rightClamp == 0) && (topClamp == 0 || bottomClamp == 0))
            parent.position = newPosition - (Vector3)mouseOffset * 0.5f;
        else
        {
            var x = Mathf.Clamp(newPosition.x - ((Vector3)mouseOffset).x * 0.5f, leftClamp, rightClamp);
            var y = Mathf.Clamp(newPosition.y - ((Vector3)mouseOffset).y * 0.5f, bottomClamp, topClamp);

            parent.position = new Vector3(x, y, 0);
        }
        print(parent.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        parent.SetAsLastSibling();
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform.parent, eventData.position, eventData.pressEventCamera, out mouseOffset);
    }

    #endregion
}
