using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILanguageButton : UIButtonBase
{
    [SerializeField] Settings settings = null;
    [SerializeField] Image flagImage = null;
    [SerializeField] TextMeshProUGUI acronymText = null;
    [SerializeField] Sprite[] flags = new Sprite[6];

    private void Start()
    {
        SetAcronymAndFlagImage();
    }

    public override void OnButtonClicked()
    {
        Translation.ChangeLanguage();
        SetAcronymAndFlagImage();
    }

    private void SetAcronymAndFlagImage()
    {
        switch (Translation.GetCurrentLanguage())
        {
            case LanguageType.en:
                acronymText.text = LanguageType.en.ToString();
                flagImage.sprite = flags[0];
                break;
            case LanguageType.es:
                acronymText.text = LanguageType.es.ToString();
                flagImage.sprite = flags[1];
                break;
            case LanguageType.zh:
                acronymText.text = LanguageType.zh.ToString();
                flagImage.sprite = flags[2];
                break;
            case LanguageType.fr:
                acronymText.text = LanguageType.fr.ToString();
                flagImage.sprite = flags[3];
                break;
            case LanguageType.ja:
                acronymText.text = LanguageType.ja.ToString();
                flagImage.sprite = flags[4];
                break;
            case LanguageType.pt:
                acronymText.text = LanguageType.pt.ToString();
                flagImage.sprite = flags[5];
                break;
            case LanguageType.UNKNOWN:
                break;
            default:
                break;
        }
        settings.languageID = Translation.currentLanguageId;
    }
}
