using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
    public int coins = 0;
    public int stars = 0;
    public float prestige = 0;
    public int skinAvailability = 0;

    /// <summary>
    /// Add the amount by the type
    /// </summary>
    /// <param name="_Amount"></param>
    public void AddCollectible(CollectibleType _CollectibleType , int _Amount)
    {
        if (_CollectibleType == CollectibleType.Coin)
        {
            coins += _Amount;
        }
        else if(_CollectibleType == CollectibleType.Star)
        {
            stars += _Amount;
        }
    }

}
