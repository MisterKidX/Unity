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
        public static Dictionary<GameObject, string> goOriginTags = new Dictionary<GameObject, string>();
        public int callbackOrder { get { return 0; } }

        public void OnPostprocessBuild(BuildReport report)
        {
            foreach (var kvp in goOriginTags)
            {
                kvp.Key.tag = goOriginTags[kvp.Key];
            }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            goOriginTags.Clear();
            var gos = GameObject.FindGameObjectsWithTag("DevelopmentOnly");

            foreach (var go in gos)
            {
                goOriginTags.Add(go, go.tag);

                if ((report.summary.options & BuildOptions.Development) != 0) { }
                else
                    go.tag = "EditorOnly";
            }
        }
    }
}
