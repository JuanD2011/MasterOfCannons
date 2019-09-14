using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer = null;
    [SerializeField] Settings settings = null;
    [SerializeField] Slider m_Slider = null;
    [SerializeField] Image m_Image = null;
    [SerializeField] AudioType m_Type = AudioType.Music;

    readonly float mutedVolume = -80f;

    /// <summary>
    /// Initialize the mixer
    /// </summary>
    public void Init()
    {
        switch (m_Type)
        {
            case AudioType.Music:
                if (!settings.isMusicActive) audioMixer.SetFloat("MusicVolume", mutedVolume);
                m_Slider.value = settings.musicSlider;
                break;
            case AudioType.SFX:
                if (!settings.isSFXActive) audioMixer.SetFloat("SFXVolume", mutedVolume);
                m_Slider.value = settings.sFXSlider;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Slider to control the volume, only works if the audio is active.
    /// </summary>
    /// <param name="_vol"></param>
    public void Slider(float _vol)
    {
        m_Slider.value = _vol;

        switch (m_Type)  
        {
            case AudioType.Music:
                audioMixer.SetFloat("MusicVolume", _vol);
                settings.musicSlider = _vol;
                if (m_Slider.value <= mutedVolume) settings.isMusicActive = false;
                else settings.isMusicActive = true;
                break;
            case AudioType.SFX:
                audioMixer.SetFloat("SFXVolume", _vol);
                settings.sFXSlider = _vol;
                if (m_Slider.value <= mutedVolume) settings.isSFXActive = false;
                else settings.isSFXActive = true;
                break;
            default:
                break;
        }
        UpdateIcon();
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
                    audioMixer.SetFloat("MusicVolume", mutedVolume);
                    settings.isMusicActive = false;
                    m_Slider.value = m_Slider.minValue;
                }
                else if (value <= mutedVolume)
                {
                    m_Slider.value = m_Slider.maxValue;
                    settings.isMusicActive = true;
                    audioMixer.SetFloat("MusicVolume", settings.musicSlider);
                }
                break;
            case AudioType.SFX:
                audioMixer.GetFloat("SFXVolume", out value);
                if (value > mutedVolume)
                {
                    audioMixer.SetFloat("SFXVolume", mutedVolume);
                    settings.isSFXActive = false;
                    m_Slider.value = m_Slider.minValue;
                }
                else if (value <= mutedVolume)
                {
                    m_Slider.value = m_Slider.maxValue;
                    settings.isSFXActive = true;
                    audioMixer.SetFloat("SFXVolume",settings.sFXSlider);
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