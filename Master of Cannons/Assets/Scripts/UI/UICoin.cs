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
        UpdateText(CollectibleType.Coin);
        CollectibleManager.OnCollectibleAdded += UpdateText;
    }

    private void UpdateText(CollectibleType _CollectibleType)
    {
        if (_CollectibleType == CollectibleType.Coin)
        {
            m_Text.text = CollectibleManager.CollectedCoins.ToString();
        }
    }
}
