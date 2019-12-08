using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILanguage : MonoBehaviour
{
    [SerializeField]
    private Settings settings = null;

    private TMP_Dropdown m_Dropdown = null;

    private void Awake() => m_Dropdown = GetComponent<TMP_Dropdown>();

    private void Start()
    {
        m_Dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        InitializeDropdown();

        Translation.OnLanguageLoaded += UpdateDropdownTexts;
    }

    private void InitializeDropdown()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < Translation.LanguageTypes.Length; i++)
        {
            options.Add(new TMP_Dropdown.OptionData(Translation.GetTextTranslated(Translation.LanguageTypes[i].ToString())));
        }

        m_Dropdown.options = options;

        m_Dropdown.value = settings.languageId;
    }

    private void UpdateDropdownTexts()
    {
        m_Dropdown.captionText.SetText(string.Format("{0}", Translation.GetTextTranslated(Translation.GetCurrentLanguage().ToString())));

        for (int i = 0; i < m_Dropdown.options.Count; i++)
        {
            m_Dropdown.options[i].text = string.Format("{0}", Translation.GetTextTranslated(Translation.LanguageTypes[i].ToString()));
        }
    }

    private void OnDropdownValueChanged(int _Value)
    {
        Translation.ChangeLanguage(_Value);
        settings.languageId = Translation.CurrentLanguageId;
    }
}
