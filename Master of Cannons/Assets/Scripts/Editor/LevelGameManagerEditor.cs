using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelGameManager))]
public class LevelGameManagerEditor : Editor
{
    SerializedProperty slider;

    protected virtual void OnEnable()
    {
        slider = serializedObject.FindProperty("slider");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((LevelGameManager)target), typeof(LevelGameManager), false);
        GUI.enabled = true;

        EditorGUILayout.PropertyField(slider);
        serializedObject.ApplyModifiedProperties();
    }
}