using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AudioManager))]
public class AudioManagerEditor : Editor
{
    SerializedProperty audioClips = null;
    SerializedProperty audioSourcesAmount = null;

    SerializedProperty audioSourceTemplate = null;
    SerializedProperty audioMixer = null;

    SerializedProperty audioSettings = null;

    SerializedProperty toggle = null;

    private void OnEnable()
    {
        audioSettings = serializedObject.FindProperty("audioSettings");
        audioClips = serializedObject.FindProperty("audioClips");

        audioSourcesAmount = serializedObject.FindProperty("audioSourcesAmount");
        audioSourceTemplate = serializedObject.FindProperty("audioSourceTemplate");

        audioMixer = serializedObject.FindProperty("audioMixer");

        toggle = serializedObject.FindProperty("initAudioSettings");
    }

    public override void OnInspectorGUI()
    {
        GUI.enabled = false;
        EditorGUILayout.ObjectField("Script", MonoScript.FromMonoBehaviour((AudioManager)target), typeof(AudioManager), false);
        GUI.enabled = true;
        serializedObject.Update();

        EditorGUILayout.PropertyField(audioClips, new GUIContent("Audio Clips", "ScriptableObject with all audioclips"));
        audioSourcesAmount.intValue = EditorGUILayout.IntField(new GUIContent("Audio sources amount", "Number of AudioSource to be instantiated"), audioSourcesAmount.intValue);
        EditorGUILayout.PropertyField(audioSourceTemplate, new GUIContent("Audio source template"));
        EditorGUILayout.PropertyField(audioMixer, new GUIContent("Audio Mixer"));

        toggle.boolValue = EditorGUILayout.Toggle(new GUIContent("There are audio settings?"), toggle.boolValue);

        if (toggle.boolValue)
        {
            EditorGUILayout.PropertyField(audioSettings, true);  
        }
        serializedObject.ApplyModifiedProperties();
    }
}
