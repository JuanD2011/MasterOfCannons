using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UILanguage : MonoBehaviour
{
    [SerializeField]
    private Settings settings = null;

    [SerializeField]
    private TextMeshProUGUI acronymText = null;

    private TMP_Dropdown m_Dropdown = null;

    private void Awake()
    {
        m_Dropdown = GetComponent<TMP_Dropdown>();

        InitializeDropdown();
    }

    protected void Start()
    {
        m_Dropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        SetAcronym();
    }

    private void InitializeDropdown()
    {
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        for (int i = 0; i < Translation.LanguageTypes.Length; i++)
        {
            options.Add(new TMP_Dropdown.OptionData(Translation.LanguageTypes[i].ToString()));
        }

        m_Dropdown.AddOptions(options);

        m_Dropdown.value = settings.languageId;
    }

    private void OnDropdownValueChanged(int _Value)
    {
        Translation.ChangeLanguage(_Value);
        SetAcronym();
    }

    private void SetAcronym()
    {
        acronymText.SetText(Translation.GetCurrentLanguage().ToString());

        settings.languageId = Translation.CurrentLanguageId;
    }
}
