using System;
using System.Collections.Generic;

[Serializable]
public class PlayerInfo
{
    public float coins;
    public float xp;
    public float skinAvailability;

    public Dictionary<string, Object> ToDictionary()
    {
        Dictionary<string, Object> result = new Dictionary<string, Object>();
        result["coins"] = coins;
        result["xp"] = xp;
        result["skinAvailability"] = skinAvailability;
        return result;
    }
}
