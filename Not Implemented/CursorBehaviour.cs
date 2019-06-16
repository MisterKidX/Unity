using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorBehaviour : MonoBehaviour 
{
    #region Fields & Autoprops

    public Texture2D regularMouse;
    public Texture2D selectMouse;

    private GraphicRaycaster _raycaster;
    private EventSystem _eventSystem;
    private bool _isSelectMode = false;

    #endregion

    #region Methods

    private void Awake()
    {
        var c = GameObject.FindGameObjectWithTag("MainGame");

        _raycaster = c.GetComponent<GraphicRaycaster>();
        _eventSystem = FindObjectOfType<EventSystem>();
    }

    private void Update()
    {
        PointerEventData data = new PointerEventData(_eventSystem);
        var results = new List<RaycastResult>();
        data.position = Input.mousePosition;
        _raycaster.Raycast(data, results);
        Selectable s;

        foreach (var result in results)
        {
            var res = result.gameObject.GetComponent<Selectable>();
            if (res && res.IsInteractable())
                _isSelectMode = true;
            else
                _isSelectMode = false;
        }

        if (_isSelectMode)
        {
            Cursor.SetCursor(selectMouse, new Vector2(128, 128), CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(regularMouse, new Vector2(128, 128), CursorMode.Auto);
        }
    }

    #endregion
}
