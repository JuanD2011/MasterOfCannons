using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MenuGameManager))]
public class MenuGameManagerEditor : Editor
{
    SerializedProperty panels;

    protected virtual void OnEnable()
    {
        panels = serializedObject.FindProperty("panels");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((MenuGameManager)target), typeof(MenuGameManager), false);
        GUI.enabled = true;

        EditorGUILayout.PropertyField(panels, true);
        serializedObject.ApplyModifiedProperties();
    }
}