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

[CustomEditor(typeof(MovingCannon))]
public class MovingCannonEditor : CannonEditor
{
    SerializedProperty startMoving;
    SerializedProperty speed;
    SerializedProperty tweenType;

    protected override void OnEnable()
    {
        base.OnEnable();

        startMoving = serializedObject.FindProperty("startMoving");
        speed = serializedObject.FindProperty("speed");
        tweenType = serializedObject.FindProperty("tweenType");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        startMoving.boolValue = EditorGUILayout.Toggle(new GUIContent("Start Moving↔"), startMoving.boolValue);
        speed.floatValue = EditorGUILayout.FloatField("Speed", speed.floatValue);
        EditorGUILayout.PropertyField(tweenType, new GUIContent("Tween Type"));

        serializedObject.ApplyModifiedProperties();
    }
}

[CustomEditor(typeof(AimingCannon))]
public class AimingCannonEditor : CannonEditor
{
    protected override void OnEnable()
    {
        base.OnEnable();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}

[CustomEditor(typeof(RotatingCannon))]
public class RotatingCannonEditor : MovingCannonEditor
{
    SerializedProperty angles;

    protected override void OnEnable()
    {
        base.OnEnable();
        angles = serializedObject.FindProperty("angles");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(angles, true);

        serializedObject.ApplyModifiedProperties();
    }
}