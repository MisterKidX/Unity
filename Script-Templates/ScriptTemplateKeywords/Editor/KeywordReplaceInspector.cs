using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

[CustomEditor(typeof(KeywordReplace))]
public class KeywordReplaceInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("CREATE SCRIPT"))
        {
            DirectoryInfo dir = new DirectoryInfo(Path.Combine(Path.GetDirectoryName(AssetDatabase.GetAssetPath(target)), "Editor"));

            File.WriteAllText(Path.Combine(dir.FullName, "KeywordReplacer.cs"), BuildFile());
            AssetDatabase.Refresh();
        }
    }

    private string BuildFile()
    {
        KeywordReplace t = target as KeywordReplace;
        string format = $"file = file.Replace(\"#KEYWORD#\", COMMAND.ToString());";
        var keywords = new List<string>(t.keywords.Count);

        foreach (var kdata in t.keywords)
        {
            var s = format.Replace("KEYWORD", kdata.keyword.ToUpper());
            s = format.Replace("COMMAND", kdata.command);
            keywords.Add(s);
        }
        StringBuilder sb = new StringBuilder();
        sb.AppendLine(@"using UnityEngine;
using UnityEditor;
public class KeywordReplacer : UnityEditor.AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace("".meta"", """");
        int index = path.LastIndexOf(""."");
        string file = path.Substring(index);
        if (file != "".cs"" && file != "".js"" && file != "".boo"") return;
        index = Application.dataPath.LastIndexOf(""Assets"");
        path = Application.dataPath.Substring(0, index) + path;
        file = System.IO.File.ReadAllText(path);");

        sb.AppendLine();
        foreach (var item in keywords)
        {
            sb.AppendLine("\t\t" + item);
        }

        sb.AppendLine(
        @"
        System.IO.File.WriteAllText(path, file);
        AssetDatabase.Refresh();
    }           
}");

        return sb.ToString();
//@"using UnityEngine;
//using UnityEditor;
//public class KeywordReplacer : UnityEditor.AssetModificationProcessor
//{
//    public static void OnWillCreateAsset(string path)
//    {
//        path = path.Replace("".meta"", """");
//        int index = path.LastIndexOf(""."");
//        string file = path.Substring(index);
//        if (file != "".cs"" && file != "".js"" && file != "".boo"") return;
//        index = Application.dataPath.LastIndexOf(""Assets"");
//        path = Application.dataPath.Substring(0, index) + path;
//        file = System.IO.File.ReadAllText(path);

//        file = file.Replace(""#CREATIONDATE#"", System.DateTime.Now + """");
//        file = file.Replace(""#PROJECTNAME#"", PlayerSettings.productName);
//        file = file.Replace(""#SMARTDEVELOPERS#"", PlayerSettings.companyName);

//        System.IO.File.WriteAllText(path, file);
//        AssetDatabase.Refresh();
//    }           
//}";
    }
}
