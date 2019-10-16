using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
    public int coins = 0;
    public int stars = 0;

    public void AddCoins(int _Coins)
    {
        coins += _Coins;
    }
}
