﻿using UnityEngine;
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

    [SerializeField]
    private Color enabledColor = new Color(0, 149, 135), disabledColor = Color.red;

    private readonly string on = "On", off = "Off";

    private const float mutedVolume = -80f;

    private TextTranslation textTranslation = null;

    /// <summary>
    /// Initialize the mixer
    /// </summary>
    public void Init()
    {
        textTranslation = GetComponentInChildren<TextTranslation>();

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