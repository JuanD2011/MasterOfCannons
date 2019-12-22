using UnityEngine;
using TMPro;

public class UICurrency : MonoBehaviour
{
    [SerializeField] private CollectibleType collectibleType = CollectibleType.None;

    private PlayerData playerData = null;

    private TextMeshProUGUI m_Text = null;

    private void Awake()
    {
        playerData = Resources.Load<PlayerData>("Scriptable Objects/Player Data");//TODO Get cached data.

        m_Text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        UpdateText(collectibleType);
        playerData.OnCollectibleAdded += UpdateText;
    }

    private void UpdateText(CollectibleType _collectibleType)
    {
        if (_collectibleType == CollectibleType.Coin)
        {
            m_Text.text = playerData.coins.ToString();
        }
    }
}
