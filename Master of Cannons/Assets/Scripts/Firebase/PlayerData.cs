using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public float xp, coins, skinAvailability;
    
    public PlayerData(float xp, float coins, float skinAvailability)
    {
        this.xp = xp;
        this.coins = coins;
        this.skinAvailability = skinAvailability;
    }
}
