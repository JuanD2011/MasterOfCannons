using UnityEngine;
using TMPro;

public class UIGameCoins : MonoBehaviour
{
    private TextMeshProUGUI m_Text = null;

    private void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateText(CollectibleType.Coin);
        CollectibleManager.onCollectibleAdded += UpdateText;
    }

    private void UpdateText(CollectibleType _CollectibleType)
    {
        if (_CollectibleType == CollectibleType.Coin)
        {
            m_Text.text = CollectibleManager.CollectedCoins.ToString();
        }
    }
}
