#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities
{
    public class IgnoreFromBuild : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public static Dictionary<int, string> goOriginTags = new Dictionary<int, string>();
        public int callbackOrder { get { return 0; } }

        public void OnPostprocessBuild(BuildReport report)
        {
            var gos = GameObject.FindGameObjectsWithTag("DevelopmentOnly");

            foreach (var go in gos)
            {
                string s;
                goOriginTags.TryGetValue(go.GetHashCode(), out s);
                if (!string.IsNullOrEmpty(s))
                    go.tag = s;
            }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            goOriginTags.Clear();
            var gos = GameObject.FindGameObjectsWithTag("DevelopmentOnly");

            foreach (var go in gos)
            {
                goOriginTags.Add(go.GetHashCode(), go.tag);

                if ((report.summary.options & BuildOptions.Development) != 0) { }
                else
                    go.tag = "EditorOnly";
            }
        }
    }
}
#endif
