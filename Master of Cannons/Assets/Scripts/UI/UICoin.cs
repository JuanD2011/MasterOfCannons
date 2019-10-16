using UnityEngine;
using TMPro;

public class UICoin : MonoBehaviour
{
    TextMeshProUGUI m_Text = null;

    private void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateText();
        CoinsManager.OnCoinAdded += UpdateText;
    }

    private void UpdateText()
    {
        m_Text.text = CoinsManager.CollectedCoins.ToString();
    }
}
