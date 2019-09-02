using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestDirection : MonoBehaviour
{
    float angle = -720;
    float speed = 45;
    int i = 0;
    Array arr;

    public Text text;
    public Button[] btns;

    private void Awake()
    {
        arr = Enum.GetValues(typeof(DirectionEnum));

        Debug.DrawLine(Vector3.zero, Vector3.up, Color.gray, 500);
        Debug.DrawLine(Vector3.zero, Vector3.down, Color.gray, 500);
        Debug.DrawLine(Vector3.zero, Vector3.right, Color.gray, 500);
        Debug.DrawLine(Vector3.zero, Vector3.left, Color.gray, 500);
        Debug.DrawLine(Vector3.zero, new Vector3(1,1,0).normalized, Color.gray, 500);
        Debug.DrawLine(Vector3.zero, new Vector3(1,-1,0).normalized, Color.gray, 500);
        Debug.DrawLine(Vector3.zero, new Vector3(-1,1,0).normalized, Color.gray, 500);
        Debug.DrawLine(Vector3.zero, new Vector3(-1,-1,0).normalized, Color.gray, 500);
    }

    private void Update()
    {
        Debug.DrawRay(Vector3.zero, new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)), Color.green);

        var dir = Direction.FromV2(new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)));

        foreach (var item in btns)
        {
            item.interactable = true;
        }

        if (Direction.AngleWithinDirectionRange(Mathf.Deg2Rad * angle, dir))
        {
            switch (dir)
            {
                case DirectionEnum.North:
                    btns[0].interactable = false;
                    break;
                case DirectionEnum.East:
                    btns[1].interactable = false;
                    break;
                case DirectionEnum.South:
                    btns[2].interactable = false;
                    break;
                case DirectionEnum.West:
                    btns[3].interactable = false;
                    break;
                case DirectionEnum.NorthEast:
                    btns[4].interactable = false;
                    break;
                case DirectionEnum.NorthWest:
                    btns[5].interactable = false;
                    break;
                case DirectionEnum.SouthWest:
                    btns[6].interactable = false;
                    break;
                case DirectionEnum.SouthEast:
                    btns[7].interactable = false;
                    break;
                default:
                    break;
            }
        }



        angle += Time.deltaTime * speed;
        text.text = angle.ToString();

        if (angle >= 720)
        {
            i++;
            angle = -720;
            if (i > arr.Length)
            {
                Debug.Break();
            }
        }
    }
}
