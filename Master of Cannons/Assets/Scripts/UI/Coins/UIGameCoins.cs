using TMPro;
using UnityEngine;

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
    }

    private void UpdateText(CollectibleType _CollectibleType)
    {
        if (_CollectibleType == CollectibleType.Coin)
        {
            m_Text.SetText(CollectibleManager.Instance.CollectedCoins.ToString());
        }
    }
}
