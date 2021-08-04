using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/*
    This script displays a developer note in the inspector.
    Methods adapted from the example in the Unity documentation.
 */

[System.Serializable]
public class DeveloperNotes : PropertyAttribute
{
    [SerializeField] string content;
    public DeveloperNotes(string s)
    {
        content = s;
    }
}


[CustomPropertyDrawer(typeof(DeveloperNotes))]
public class DeveloperNotesDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;
        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(position, property.FindPropertyRelative("content"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
