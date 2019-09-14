using UnityEngine;

[CreateAssetMenu(fileName = "AudioClips", menuName = "AudioClips")]
public class AudioClips : ScriptableObject
{
    [Header("Music")]
    public AudioClip inGameMusic = null;

    [Header("UI")]
    public AudioClip defaultButton = null;
    public AudioClip acceptButton = null;
    public AudioClip cancelButton = null;
    public AudioClip notificationAlert = null;
}