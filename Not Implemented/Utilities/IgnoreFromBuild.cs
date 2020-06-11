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
    public class IgnoreFromBuild : MonoBehaviour, IPreprocessBuildWithReport
    {
        public bool keepIfDevelopmentBuild = true;

        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            var ignores = GameObject.FindObjectsOfType<IgnoreFromBuild>();

            foreach (var ignore in ignores)
            {
                if (ignore.keepIfDevelopmentBuild && (report.summary.options & BuildOptions.Development) != 0) { }
                else
                    ignore.tag = "EditorOnly";

            }
        }
    }
}
#endif


