using UnityEditor;
using GoogleServices;
using UnityEngine;

namespace GoogleServices
{
    [CustomEditor(typeof(GoogleSheetsService))]
    public class GoogleSheetsServiceEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            var cred = serializedObject.FindProperty("credentials");

            base.OnInspectorGUI();
            if (GUILayout.Button("Credentials"))
            {
                cred.stringValue = EditorUtility.OpenFilePanel("Open Credentials", ".../", "json");

                serializedObject.ApplyModifiedProperties();
            }
            
            if (!cred.stringValue.EndsWith(".json"))
            {
                EditorGUILayout.HelpBox("You didn't select a json file!", MessageType.Warning);
            }
        }

    }
}