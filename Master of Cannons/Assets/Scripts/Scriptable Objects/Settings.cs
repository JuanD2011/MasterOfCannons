using UnityEngine;

[CreateAssetMenu(fileName ="Settings", menuName ="Settings")]
public class Settings : ScriptableObject
{
    [Header("Configuration Settings")]
    public bool isMusicActive;
    public bool isSFXActive;

    public float musicSlider;
    public float sFXSlider;

    [Header("Language")]
    public byte languageID = 0;

    public void MusicSlider(float _musicVol) { musicSlider = _musicVol; }
    public void SFXSlider(float _sFXVol) { sFXSlider = _sFXVol; }
}
