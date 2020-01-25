using UnityEngine;
using TMPro;

/// <summary>
/// Shows total collectible amount each time is on enable
/// </summary>
public class UICollectible : MonoBehaviour
{
    [SerializeField] private CollectibleType collectibleType = CollectibleType.None;

    private PlayerData playerData = null;

    protected TextMeshProUGUI m_Text = null;

    private void Awake()
    {
        playerData = Resources.Load<PlayerData>("Scriptable Objects/Player Data");//TODO Get cached data.

        m_Text = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        UpdateText(collectibleType);
    }

    private void UpdateText(CollectibleType _collectibleType)
    {
        switch (_collectibleType)
        {
            case CollectibleType.Coin:
                m_Text.SetText(playerData.coins.ToString());
                break;
            case CollectibleType.Star:
                m_Text.SetText(playerData.stars.ToString());
                break;
            case CollectibleType.None:
                break;
            default:
                break;
        }
    }
}
