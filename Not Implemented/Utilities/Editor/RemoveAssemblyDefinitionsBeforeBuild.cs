using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.Compilation;
using UnityEngine;

public static class RemoveAssemblyDefinitionsBeforeBuild
{
    //[MenuItem("Tools/List Player Assemblies in Console")]
    //public static void PrintAssemblyNames()
    //{
    //    UnityEngine.Debug.Log("== Player Assemblies ==");
    //    Assembly[] playerAssemblies =
    //        CompilationPipeline.GetAssemblies(AssembliesType.Player);

    //    foreach (var assembly in playerAssemblies)
    //    {
    //        UnityEngine.Debug.Log(assembly.name);
    //    }
    //}
}

//public class Test : IPreprocessBuildWithReport, IPostprocessBuildWithReport
//{
//    public int callbackOrder { get { return 0; } }

//    public void OnPostprocessBuild(BuildReport report)
//    {
//        Debug.Log("Post: " + JsonUtility.ToJson(report, true));
//    }

//    public void OnPreprocessBuild(BuildReport report)
//    {
//        Debug.Log("Pre: " + JsonUtility.ToJson(report.steps, true));
//    }
//}
