using UnityEngine;
using TMPro;

public class UICoins : MonoBehaviour
{
    [SerializeField] private CollectibleType collectibleType = CollectibleType.None;

    private PlayerData playerData = null;

    protected TextMeshProUGUI m_Text = null;

    private void Awake()
    {
        playerData = Resources.Load<PlayerData>("Scriptable Objects/Player Data");//TODO Get cached data.

        m_Text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateText(collectibleType);
    }

    private void UpdateText(CollectibleType _collectibleType)
    {
        if (_collectibleType == CollectibleType.Coin)
        {
            m_Text.SetText(playerData.coins.ToString());
        }
    }
}
