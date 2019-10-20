using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    PlayerData playerData = null;

    public static byte CollectedCoins { get; private set; } = 0;
    public static byte CollectedStars { get; private set; } = 0;

    public static event Delegates.Action<CollectibleType> OnCollectibleAdded = null;

    private void Awake()
    {
        OnCollectibleAdded = null;
        playerData = Resources.Load<PlayerData>("Scriptable Objects/Player Data");

        CollectedCoins = 0; CollectedStars = 0;
    }

    private void Start()
    {
        Collectible.OnCollected += CollectibleCollected;
        Referee.OnGameOver += UpdateCollectibles;
    }

    /// <summary>
    /// Add current coins to scriptable object
    /// </summary>
    public void UpdateCollectibles()
    {
        playerData.AddCollectible(CollectibleType.Coin, CollectedCoins);
        playerData.AddCollectible(CollectibleType.Star, CollectedStars);

        Memento.SaveData(playerData);
    } 

    private void CollectibleCollected(CollectibleType _CollectibleType)
    {
        if (_CollectibleType == CollectibleType.Coin)
        {
            CollectedCoins += 1;
            OnCollectibleAdded(CollectibleType.Coin);
        }
        else if (_CollectibleType == CollectibleType.Star)
        {
            CollectedStars += 1;
            OnCollectibleAdded(CollectibleType.Star);
        }
    }
}
