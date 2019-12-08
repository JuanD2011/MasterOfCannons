using UnityEngine;

[CreateAssetMenu(fileName ="Settings", menuName ="Settings")]
public class Settings : ScriptableObject
{
    [Header("Configuration Settings")]
    public bool isMusicActive = false;
    public bool isSFXActive = false;

    [Header("Language")]
    public int languageId = 0;

    [Header("User Account Settings")]
    public bool hasFacebookLinked = false;

    [Header("Level selection")]
    public float scrollPosition = 0f;
}
