using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

public class UnityDelegate : MonoBehaviour
{
    public string methodToCall;

    private void Start()
    {
        var obj = this.GetType().GetMethod(methodToCall, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
             .Invoke(this, new object[0]);
    }

    void Test1()
    {
        Debug.Log("test1");
    }

    void Test2()
    {
        Debug.Log("test2");
    }
}
