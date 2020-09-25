using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(UnityDelegateNotMB))]
public class UnityDelegatePropertyDrawer : PropertyDrawer
{
    static string[] methods;
    static string[] ignoreMethods = new string[] { "Start", "Update", "OnClick" };

    static UnityDelegatePropertyDrawer()
    {
        methods =
            typeof(UnityDelegateNotMB)
                .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public) // Instance methods, both public and private/protected
                .Where(x => x.DeclaringType == typeof(UnityDelegateNotMB)) // Only list methods defined in our own class
                .Where(x => x.GetParameters().Length == 0) // Make sure we only get methods with zero argumenrts
                .Where(x => !ignoreMethods.Any(n => n == x.Name)) // Don't list methods in the ignoreMethods array (so we can exclude Unity specific methods, etc.)
                .Select(x => x.Name)
                .ToArray();
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        var prop = property.FindPropertyRelative("methodToCall");
        int index;
        
        try
        {
            index = methods
                .Select((v, i) => new { Name = v, Index = i })
                    .First(x => x.Name == prop.stringValue)
                    .Index;
        }
        catch
        {
            index = 0;
        }

        Rect popPos = new Rect(position.x, position.y, position.width * 0.6f, position.height);
        var pop = EditorGUI.Popup(popPos, index, methods);
        prop.stringValue = methods[pop];

        EditorGUI.EndProperty();
    }
}
