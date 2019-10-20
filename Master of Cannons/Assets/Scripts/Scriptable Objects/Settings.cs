using UnityEngine;

[CreateAssetMenu(fileName ="Settings", menuName ="Settings")]
public class Settings : ScriptableObject
{
    [Header("Configuration Settings")]
    public bool isMusicActive = false;
    public bool isSFXActive = false;

    public float musicSlider = 0f;
    public float sFXSlider = 0f;
    
    [Header("Language")]
    public byte languageID = 0;

    [Header("User Account Settings")]
    public bool hasFacebookLinked;
    public int defaultScene = 0;

    public bool hasFacebookLinked = false;

    [Header("Level selection")]
    public float scrollPosition = 0f;


    public void MusicSlider(float _musicVol) { musicSlider = _musicVol; }
    public void SFXSlider(float _sFXVol) { sFXSlider = _sFXVol; }
}
