using System;
using System.Collections.Generic;

[Serializable]
public class PlayerInfo
{
    public float coins;
    public float prestige;
    public float skinAvailability;

    public Dictionary<string, Object> ToDictionary()
    {
        Dictionary<string, Object> result = new Dictionary<string, Object>();
        result["coins"] = coins;
        result["prestige"] = prestige;
        result["skinAvailability"] = skinAvailability;
        return result;
    }
}
