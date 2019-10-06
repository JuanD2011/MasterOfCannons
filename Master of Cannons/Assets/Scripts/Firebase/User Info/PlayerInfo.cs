using System;
using System.Collections.Generic;

[Serializable]
public class PlayerInfo
{
    public int coins;
    public float prestige;
    public int skinAvailability;
    public int stars;

    public Dictionary<string, Object> ToDictionary()
    {
        Dictionary<string, Object> result = new Dictionary<string, Object>();
        result["coins"] = coins;
        result["prestige"] = prestige;
        result["skinAvailability"] = skinAvailability;
        return result;
    }
}
