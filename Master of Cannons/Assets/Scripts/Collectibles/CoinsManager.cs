using UnityEngine;

public class CoinsManager : MonoBehaviour
{
    PlayerData playerData = null;

    public static byte CollectedCoins { get; private set; } = 0;

    public static event Delegates.Action OnCoinAdded = null;

    private void Awake()
    {
        OnCoinAdded = null;
        playerData = Resources.Load<PlayerData>("Scriptable Objects/Player Data");
        CollectedCoins = 0;
    }

    private void Start()
    {
        Collectible.OnCollected += CoinOnCollected;
        Referee.OnGameOver += UpdateCoins;
    }

    private void UpdateCoins() => playerData.AddCoins(CollectedCoins);

    private void CoinOnCollected(CollectibleType _CollectibleType)
    {
        if (_CollectibleType != CollectibleType.Coin) return;

        CollectedCoins += 1;
        OnCoinAdded();
    }
}
