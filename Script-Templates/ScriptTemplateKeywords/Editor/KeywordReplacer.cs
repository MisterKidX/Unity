using UnityEngine;
using UnityEditor;
public class KeywordReplacer : UnityEditor.AssetModificationProcessor
{
    public static void OnWillCreateAsset(string path)
    {
        path = path.Replace(".meta", "");
        int index = path.LastIndexOf(".");
        string file = path.Substring(index);
        if (file != ".cs" && file != ".js" && file != ".boo") return;
        index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;
        file = System.IO.File.ReadAllText(path);
        // This is where you can insert your custom keywords
        
		file = file.Replace("#CREATIONDATE#", System.DateTime.Now.ToString());
		file = file.Replace("#SOMETHING#", System.DateTime.Now.ToString());

        System.IO.File.WriteAllText(path, file);
        AssetDatabase.Refresh();
    }           
}
