using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : UIButtonBase
{
    [SerializeField] private Settings settings = null;

    [SerializeField] private Image m_Image = null;
    [SerializeField] private Color enabledColor = new Color(0, 149, 135), disabledColor = Color.red;

    [SerializeField] private AudioType m_Type = AudioType.None;

    private TextTranslation textTranslation = null;

    private const string on = "On", off = "Off";

    protected override void Awake()
    {
        textTranslation = GetComponentInChildren<TextTranslation>();
        base.Awake();
    }

    /// <summary>
    /// Intialize UI
    /// </summary>
    public void OnEnable()
    {
        UpdateUI();
    }

    public override void OnButtonClicked()
    {
        AudioManager.Instance.MuteAudio(m_Type, UpdateUI);
    }

    private void UpdateUI()
    {
        switch (m_Type)
        {
            case AudioType.Music:
                if (settings.isMusicActive)
                {
                    textTranslation.TextID = on;
                    m_Image.color = enabledColor;
                }
                else
                {
                    textTranslation.TextID = off;
                    m_Image.color = disabledColor;
                }
                break;
            case AudioType.SFX:
                if (settings.isSFXActive)
                {
                    textTranslation.TextID = on;
                    m_Image.color = enabledColor;
                }
                else
                {
                    textTranslation.TextID = off;
                    m_Image.color = disabledColor;
                }
                break;
            default:
                break;
        }
        textTranslation.UpdateText();
    }
}