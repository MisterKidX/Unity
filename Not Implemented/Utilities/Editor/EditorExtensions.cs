using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif

public class EditorExtensions : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("Tools/Gavra Games/Reset Battle Dice Tutorial")]
    public static void ResetBattleDiceTurotial()
    {
        PlayerPrefs.SetInt("FinishedBattleDiceTutorial", 0);
    }

    [MenuItem("Tools/Gavra Games/Complete Battle Dice Tutorial")]
    public static void CompleteBattleDiceTurotial()
    {
        PlayerPrefs.SetInt("FinishedBattleDiceTutorial", 1);
    }

    [MenuItem("Tools/Gavra Games/Load Main Menu")]
    public static void LoadMainMenu()
    {
        var sceneName = "Multiplayer Main Menu";
        if (Application.isPlaying)
            SceneManager.LoadScene(sceneName);
        else
            EditorSceneManager.OpenScene(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(string.Format("{0} t:scene", sceneName))[0]));
    }
#endif
}
