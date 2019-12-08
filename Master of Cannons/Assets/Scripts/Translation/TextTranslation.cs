using UnityEngine;
using TMPro;

public class TextTranslation : MonoBehaviour
{
    [SerializeField]
    private string textID = "";

    [SerializeField]
    private bool initTextInStart = true;

    private TextMeshProUGUI text = null;

    public string TextID { get => textID; set => textID = value; }

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        if (initTextInStart)
        {
            UpdateText(); 
        }

        Translation.OnLanguageLoaded += UpdateText;
    }

    /// <summary>
    /// Update text with the TextID of the class
    /// </summary>
    public void UpdateText()
    {
        if (text != null && Translation.Fields.ContainsKey(TextID)) text.SetText(Translation.Fields[TextID]);
    }
}
