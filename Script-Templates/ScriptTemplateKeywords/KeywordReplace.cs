using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class KeywordReplace : ScriptableObject
{
    public List<KeywordData> keywords = new List<KeywordData>();
}

[Serializable]
public class KeywordData
{
    [SerializeField]
    public string keyword;
    [SerializeField]
    public string command;

    public KeywordData(string keyword, string command)
    {
        this.keyword = keyword;
        this.command = command;
    }
}
