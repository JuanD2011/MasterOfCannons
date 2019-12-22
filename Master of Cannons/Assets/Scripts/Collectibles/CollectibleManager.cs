using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    private PlayerData playerData = null;

    public static byte CollectedCoins { get; private set; } = 0;

    public static event Delegates.Action<CollectibleType> onCollectibleAdded = null;

    private void Awake()
    {
        onCollectibleAdded = null;
        playerData = Resources.Load<PlayerData>("Scriptable Objects/Player Data");//TODO Load this from firebase

        CollectedCoins = 0;
    }

    private void Start()
    {
        Collectible.onCollected += CollectibleCollected;
        Referee.onGameOver += UpdateCollectibles;
    }

    /// <summary>
    /// Add current coins to scriptable object
    /// </summary>
    public void UpdateCollectibles(LevelStatus _levelStatus)
    {
        playerData.AddCollectible(CollectibleType.Coin, CollectedCoins);

        Memento.SaveData(playerData);
    }

    /// <summary>
    /// This function is principally called when the user reset the world or go back to menu scene
    /// </summary>
    public void UpdateCoins()
    {
        playerData.AddCollectible(CollectibleType.Coin, CollectedCoins);
        Memento.SaveData(playerData);
    }

    private void CollectibleCollected(CollectibleType _CollectibleType)
    {
        if (_CollectibleType == CollectibleType.Coin)
        {
            CollectedCoins += 1;
            onCollectibleAdded(CollectibleType.Coin);
        }
    }

    private void OnApplicationQuit()
    {
        UpdateCoins();
    }
}
