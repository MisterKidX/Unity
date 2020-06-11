using MP;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MP.Matchmaker))]
public class MatchmakerInspector : Editor
{
    SerializedProperty _selection;
    string[] _queueOptions;

    private void OnEnable()
    {
        _selection = serializedObject.FindProperty("_selection");
        _queueOptions = Matchmaker.queueOptions;
    }

    public override void OnInspectorGUI()
    {
        _selection.intValue = EditorGUILayout.Popup("Queue", _selection.intValue, _queueOptions);
        serializedObject.ApplyModifiedProperties();
        serializedObject.Update();
        base.OnInspectorGUI();
    }
}
