using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MenuGameManager))]
public class MenuGameManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MenuGameManager)target), typeof(MenuGameManager), false);
        GUI.enabled = true;

        serializedObject.ApplyModifiedProperties();
    }
}