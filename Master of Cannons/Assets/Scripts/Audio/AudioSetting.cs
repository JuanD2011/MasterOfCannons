using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer = null;

    [SerializeField]
    private Settings settings = null;

    [SerializeField]
    private Image m_Image = null;

    [SerializeField]
    private AudioType m_Type = AudioType.Music;

    private const float mutedVolume = -80f;

    /// <summary>
    /// Initialize the mixer
    /// </summary>
    public void Init()
    {
        switch (m_Type)
        {
            case AudioType.Music:
                if (!settings.isMusicActive) audioMixer.SetFloat("MusicVolume", mutedVolume);
                break;
            case AudioType.SFX:
                if (!settings.isSFXActive) audioMixer.SetFloat("SFXVolume", mutedVolume);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Function to mute audio
    /// </summary>
    public void MuteAudio()
    {
        float value = 0f;
        switch (m_Type)   
        {
            case AudioType.Music:
                audioMixer.GetFloat("MusicVolume", out value);
                if (value > mutedVolume)
                {
                    settings.isMusicActive = false;
                    audioMixer.SetFloat("MusicVolume", mutedVolume);
                }
                else if (value <= mutedVolume)
                {
                    settings.isMusicActive = true;
                    audioMixer.SetFloat("MusicVolume", 0f);
                }
                break;
            case AudioType.SFX:
                audioMixer.GetFloat("SFXVolume", out value);
                if (value > mutedVolume)
                {
                    audioMixer.SetFloat("SFXVolume", mutedVolume);
                    settings.isSFXActive = false;
                }
                else if (value <= mutedVolume)
                {
                    settings.isSFXActive = true;
                    audioMixer.SetFloat("SFXVolume", 0f);
                }
                break;
            default:
                break;
        }
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        switch (m_Type)
        {
            case AudioType.Music:
                if (settings.isMusicActive)
                {
                    m_Image.color = Color.white;
                }
                else
                {
                    m_Image.color = Color.red;
                }
                break;
            case AudioType.SFX:
                if (settings.isSFXActive)
                {
                    m_Image.color = Color.white;
                }
                else
                {
                    m_Image.color = Color.red;
                }
                break;
            default:
                break;
        }
    }
}