using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer = null;

    [SerializeField] private Settings settings = null;

    [SerializeField] private Image m_Image = null;

    [SerializeField] private AudioType m_Type = AudioType.None;

    [SerializeField] private Color enabledColor = new Color(0, 149, 135), disabledColor = Color.red;

    private const string on = "On", off = "Off";
    private const string mixerMusicVolume = "MusicVolume", mixerSFXVolume = "SFXVolume";

    private const float mutedVolume = -80f;

    private TextTranslation textTranslation = null;

    private void Awake()
    {
        textTranslation = GetComponentInChildren<TextTranslation>();
    }

    private void Start()
    {
        Init();
    }

    /// <summary>
    /// Initialize the mixer and Intialize the UI
    /// </summary>
    public void Init()
    {
        switch (m_Type)
        {
            case AudioType.Music:
                if (!settings.isMusicActive) audioMixer.SetFloat(mixerMusicVolume, mutedVolume);
                break;
            case AudioType.SFX:
                if (!settings.isSFXActive) audioMixer.SetFloat(mixerSFXVolume, mutedVolume);
                break;
            default:
                break;
        }
        UpdateUI();
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
                audioMixer.GetFloat(mixerMusicVolume, out value);
                if (value > mutedVolume)
                {
                    settings.isMusicActive = false;
                    audioMixer.SetFloat(mixerMusicVolume, mutedVolume);
                }
                else if (value <= mutedVolume)
                {
                    settings.isMusicActive = true;
                    audioMixer.SetFloat(mixerMusicVolume, 0f);
                }
                break;
            case AudioType.SFX:
                audioMixer.GetFloat(mixerSFXVolume, out value);
                if (value > mutedVolume)
                {
                    audioMixer.SetFloat(mixerSFXVolume, mutedVolume);
                    settings.isSFXActive = false;
                }
                else if (value <= mutedVolume)
                {
                    settings.isSFXActive = true;
                    audioMixer.SetFloat(mixerSFXVolume, 0f);
                }
                break;
            default:
                break;
        }
        UpdateUI();
    }

    private void UpdateUI()
    {
        switch (m_Type)
        {
            case AudioType.Music:
                if (settings.isMusicActive)
                {
                    m_Image.color = enabledColor;
                    textTranslation.TextID = on;
                }
                else
                {
                    m_Image.color = disabledColor;
                    textTranslation.TextID = off;
                }
                textTranslation.UpdateText();
                break;
            case AudioType.SFX:
                if (settings.isSFXActive)
                {
                    m_Image.color = enabledColor;
                    textTranslation.TextID = on;
                }
                else
                {
                    m_Image.color = disabledColor;
                    textTranslation.TextID = off;
                }
                textTranslation.UpdateText();
                break;
            default:
                break;
        }
    }
}