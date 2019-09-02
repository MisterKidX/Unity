using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Curve))]
public class CurveInspector : Editor
{
    private Color _begin =  Color.white;
    private Color _end = Color.white;
    private static float _counter = 0;
    Transform g = null;
    bool _toUpdate;
    Vector4 v4 = Vector4.zero;

    public override void OnInspectorGUI()
    {
        var curve = target as Curve;
        _begin = Color.green;
        _end = Color.blue;

        base.OnInspectorGUI();
        Texture2D gradient = new Texture2D(200, 200);
        SetTextureByCurve(gradient, _begin, _end);

        EditorGUI.DrawPreviewTexture(EditorGUILayout.GetControlRect(
            GUILayout.MinHeight(200),
            GUILayout.MaxWidth(200)),
            gradient);

         v4 = EditorGUILayout.Vector4Field("Bezier", v4);

        if (GUILayout.Button("Cubic Bezier To Animation Curve"))
        {
            //var p = serializedObject.FindProperty("curve");
            var keys = curve.curve.keys;
            var key1 = keys[0];
            var key2 = keys[1];

            key1.value = 0;
            key1.time = 0;
            AnimationUtility.SetKeyLeftTangentMode(curve.curve, 0, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyRightTangentMode(curve.curve, 0, AnimationUtility.TangentMode.Free);
            AnimationUtility.SetKeyRightTangentMode(curve.curve, 1, AnimationUtility.TangentMode.Linear);
            AnimationUtility.SetKeyLeftTangentMode(curve.curve, 1, AnimationUtility.TangentMode.Free);
            key1.weightedMode = WeightedMode.Out;
            key2.weightedMode = WeightedMode.In;

            key1.outWeight = v4.x;
            key1.outTangent = v4.y/v4.x;
            key2.inWeight = 1 - v4.z;
            key2.inTangent = (1 - v4.w)/(1 - v4.z);
            keys[0] = key1;
            keys[1] = key2;

            curve.curve.keys = keys;
            //p.animationCurveValue.keys[0] = key1;
            //Debug.Log(key1.outWeight);
            //Debug.Log(key1.outTangent);
            //Debug.Log(key1.inWeight);
            //Debug.Log(key1.inTangent);
            //key1.inTangent = 0;
            EditorUtility.SetDirty(target);

        }
    }

    private void SetTransperant(Texture2D tex)
    {
        for (int i = 0; i < 200; i++)
        {
            for (int j = 0; j < 200; j++)
            {
                tex.SetPixel(i, j, Color.clear);
            }
        }
    }

    private void SetTextureByCurve(Texture2D tex, Color begin, Color end)
    {
        var c = target as Curve;

        for (int i = 0; i < 200; i++)
        {
            var time = i / 200f;
            var val = c.curve.Evaluate(time);

            for (int j = 0; j < 200; j++)
            {
                tex.SetPixel(j, i, new Color(
                Mathf.Abs(_end.r * val - _begin.r * (1-val)),
                Mathf.Abs(_end.g * val - _begin.g * (1-val)),
                Mathf.Abs(_end.b * val - _begin.b * (1-val))));
            }
        }
        tex.Apply();
    }
}
