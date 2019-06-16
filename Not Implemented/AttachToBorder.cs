using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachToBorder : MonoBehaviour
{
    [Flags]
    public enum Direction {Top = 1 << 0, Bottom= 1 << 1, Right= 1 << 2, Left= 1 << 3 }

    #region Fields & Properties

    public Direction direction;
    public bool toResize = false;
    public float baseSize = 1;

    private Camera _mainCam;

    #endregion

    #region Unity Locals

    private void Awake()
    {
        _mainCam = Camera.main;
        StickToBorder();
    }

    private void StickToBorder()
    {
        var campose = _mainCam.transform.position;
        // Aspect bigger than one is landscape, smaller is portrait.
        var aspectX = _mainCam.aspect > 1 ? 1 : _mainCam.aspect;
        var aspectY = _mainCam.aspect < 1 ? 1 : _mainCam.aspect;

        sbyte modX = direction == Direction.Right ? (sbyte)1 : (sbyte)-1;
        sbyte modY = direction == Direction.Top ? (sbyte)1 : (sbyte)-1;

        switch (direction)
        {
            case Direction.Top:
                modX = 0;
                modY = 1;
                break;
            case Direction.Bottom:
                modX = 0;
                modY = -1;
                break;
            case Direction.Right:
                modX = 1;
                modY = 0;
                break;
            case Direction.Left:
                modX = -1;
                modY = 0;
                break;
            default:
                throw new NotImplementedException();
        }

        transform.position = new Vector3
            (campose.x + _mainCam.orthographicSize * aspectX * modX + baseSize/2 * modX,
            campose.y + _mainCam.orthographicSize * aspectY * modY + baseSize/2 * modY,0);

        if (toResize && (direction & (Direction.Right | Direction.Left)) != 0)
            transform.localScale = new Vector3(1, _mainCam.orthographicSize * aspectY * 2, 1);
        else if (toResize && (direction & (Direction.Top | Direction.Bottom)) != 0)
            transform.localScale = new Vector3(_mainCam.orthographicSize * aspectX * 2, 1, 1);
    }

    #endregion
}
