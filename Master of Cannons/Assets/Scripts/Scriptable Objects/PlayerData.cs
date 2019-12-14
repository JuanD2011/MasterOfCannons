using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
    public int coins = 0;
    public int stars = 0;

    public bool defaultCharacterSet = false;

    public PlayerSkin currentCharacter = null;

    public event Delegates.Action<CollectibleType> OnCollectibleAdded = null;

    /// <summary>
    /// Add the amount by the type
    /// </summary>
    /// <param name="_Amount"></param>
    public void AddCollectible(CollectibleType _CollectibleType , int _Amount)
    {
        if (_CollectibleType == CollectibleType.Coin)
        {
            coins += _Amount;
            OnCollectibleAdded?.Invoke(CollectibleType.Coin);
        }
        else if(_CollectibleType == CollectibleType.Star)
        {
            stars += _Amount;
            OnCollectibleAdded?.Invoke(CollectibleType.Star);
        }
    }
}
