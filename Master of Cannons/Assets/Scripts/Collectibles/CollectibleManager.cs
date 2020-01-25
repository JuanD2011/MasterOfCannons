using UnityEngine;

public class CollectibleManager : Singleton<CollectibleManager>
{
    private PlayerData playerData = null;

    public byte CollectedCoins { get; private set; } = 0;

    protected override void OnAwake()
    {
        playerData = Resources.Load<PlayerData>("Scriptable Objects/Player Data");//TODO Load this from firebase

        CollectedCoins = 0;
    }

    private void Start()
    {
        Collectible.onCollected += CollectibleCollected;
        Referee.OnGameOver += UpdateCoins;
    }

    /// <summary>
    /// Add current coins to scriptable object
    /// </summary>
    public void UpdateCoins(LevelStatus _levelStatus)
    {
        playerData.coins += CollectedCoins;
        Memento.SaveData(playerData);
    }

    /// <summary>
    /// This function is principally called when the user reset the world or go back to menu scene
    /// </summary>
    public void UpdateCoins()
    {
        playerData.coins += CollectedCoins;
        Memento.SaveData(playerData);
    }

    private void CollectibleCollected(CollectibleType _collectibleType)
    {
        if (_collectibleType == CollectibleType.Coin)
        {
            CollectedCoins += 1;
        }
    }

    private void OnApplicationQuit()
    {
#if !UNITY_EDITOR
        UpdateCoins(); 
#endif
    }
}
