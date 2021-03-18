// --- PBGamesStudio - PhantomBeasts ---
//   Dor Ben Dor (agesonera@gmail.com)
//   3/18/2021 9:31:53 AM
// ------------------------------------

#if UNITY_EDITOR

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PhantomBeasts.Utilities
{
    public class SkinnedMeshBoneReorganizer : EditorWindow
    {
        static SerializedObject serObj;

        [MenuItem("Tools/3D/Update Skinned Mesh")]
        static void Init()
        {
            var so = CreateInstance<SkinnedMeshSerializedProperties>();
            serObj = new SerializedObject(so);

            // Get existing open window or if none, make a new one:
            SkinnedMeshBoneReorganizer window = (SkinnedMeshBoneReorganizer)GetWindow(typeof(SkinnedMeshBoneReorganizer));
            window.Show();
        }

        void OnGUI()
        {
            var p1 = serObj.FindProperty("ChangeThis");
            var p2 = serObj.FindProperty("ToThis");

            EditorGUILayout.Space(30);
            EditorGUILayout.PropertyField(p1);
            EditorGUILayout.Space(30);
            EditorGUILayout.PropertyField(p2);

            if (p1.objectReferenceValue != null & p2.objectReferenceValue != null)
            {
                if (GUILayout.Button("Change"))
                {
                    UpdateMeshRenderer(p1.objectReferenceValue as SkinnedMeshRenderer, p2.objectReferenceValue as SkinnedMeshRenderer);
                }
            }

            serObj.ApplyModifiedProperties();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void UpdateMeshRenderer(SkinnedMeshRenderer from, SkinnedMeshRenderer to)
        {
            from.sharedMesh = to.sharedMesh;

            Transform[] children = from.transform.parent.GetComponentsInChildren<Transform>(true);

            Transform[] bones = new Transform[to.bones.Length];

            for (int i = 0; i < to.bones.Length; i++)
            {
                bones[i] = Array.Find(children, c => c.name == to.bones[i].name);
            }

            from.bones = bones;
        }

        [Serializable]
        private class SkinnedMeshSerializedProperties : ScriptableObject
        {
            public SkinnedMeshRenderer ChangeThis;
            public SkinnedMeshRenderer ToThis;
        }
    }

    
}

#endif