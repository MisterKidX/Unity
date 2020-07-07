using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Utilities
{
    public class EditorUtilities : MonoBehaviour
    {
        static void SetByName(object obj, string name)
        {
            switch (obj)
            {
                case string s:
                    EditorPrefs.SetString(name, obj as string);
                    break;
                case int i:
                    EditorPrefs.SetInt(name, (int)obj);
                    break;
                case bool b:
                    EditorPrefs.SetBool(name, (bool)obj);
                    break;
            }
        }

        static object GetByName(object obj, string name, object @default = null)
        {
            object val = null;
            switch (obj)
            {
                case string s:
                    if (@default != null)
                        val = EditorPrefs.GetString(name, @default as string);
                    else
                        val = EditorPrefs.GetString(name);
                    break;
                case int i:
                    if (@default != null)
                        val = EditorPrefs.GetInt(name, (int)@default);
                    else
                        val = EditorPrefs.GetInt(name);
                    break;
                case bool b:
                    if (@default != null)
                        val = EditorPrefs.GetBool(name, (bool)@default);
                    else
                        val = EditorPrefs.GetBool(name);
                    break;
                default:
                    Debug.Log("Could Not parse for editorprefs save.");
                    break;
            }

            return val;
        }
    }
}

