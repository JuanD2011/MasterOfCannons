using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Cannon))]
public class CannonEditor : Editor
{
    SerializedProperty shootForce;
    SerializedProperty wickLength;
    SerializedProperty doCatchRotation;
    SerializedProperty catchRotation;

    protected virtual void OnEnable()
    {
        shootForce = serializedObject.FindProperty("shootForce");
        wickLength = serializedObject.FindProperty("wickLength");

        doCatchRotation = serializedObject.FindProperty("doCatchRotation");
        catchRotation = serializedObject.FindProperty("catchRotation");
    }

    protected virtual void ShowScript()
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((Cannon)target), typeof(Cannon), false);
        GUI.enabled = true;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        shootForce.floatValue = EditorGUILayout.FloatField("Shoot force", shootForce.floatValue);
        wickLength.floatValue = EditorGUILayout.FloatField("Wick length", wickLength.floatValue);

        doCatchRotation.boolValue = EditorGUILayout.Toggle(new GUIContent("Do catch rotation?"), doCatchRotation.boolValue);

        if (doCatchRotation.boolValue)
        {
            EditorGUILayout.PropertyField(catchRotation);
        }
        serializedObject.ApplyModifiedProperties();
    }
}