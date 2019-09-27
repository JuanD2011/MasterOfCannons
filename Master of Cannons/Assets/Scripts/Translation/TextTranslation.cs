using UnityEngine;
using TMPro;

public class TextTranslation : MonoBehaviour
{
    [SerializeField] string textID = "";
    TextMeshProUGUI text = null;

    public string TextID { get => textID; set => textID = value; }

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        if (text != null) text.text = Translation.Fields[TextID];

        Translation.OnLanguageLoaded += UpdateText;
    }

    private void UpdateText()
    {
        if (text != null) text.text = Translation.Fields[TextID];
    }
}
