using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Player Data")]
public class PlayerData : ScriptableObject
{
    public int coins = 0;
    public int stars = 0;

    public bool defaultCharacterSet = false;

    public PlayerSkin currentCharacter = null;
}
