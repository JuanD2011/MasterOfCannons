using TMPro;

public class UIGameCoins : UICoins
{
    private void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateText(CollectibleType.Coin);
        CollectibleManager.Instance.OnCollectibleAdded += UpdateText;
    }

    private void UpdateText(CollectibleType _CollectibleType)
    {
        if (_CollectibleType == CollectibleType.Coin)
        {
            m_Text.SetText(CollectibleManager.Instance.CollectedCoins.ToString());
        }
    }
}
