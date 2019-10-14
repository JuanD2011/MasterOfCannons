using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager DM = null;
    public Settings settings;
    [SerializeField] bool clearSettingsData = false;

    public PlayerData playerData = null;
    public Dictionary<string, string> playerDataDict;
    public const string coinsStr = "coins", prestigeStr = "prestige", starsStr = "stars", skinAvailabilityStr = "skinAvailability";
    public const string timeChestWasOpenedStr = "timeChestWasOpened";
    private void Awake()
    {
        if(clearSettingsData) Memento.ClearData(settings);
        if (DM == null) DM = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        Memento.LoadData(settings);        

    }

    public void InitializePlayerData(Dictionary<string,string> playerDataDict)
    {
        playerData.coins = int.Parse(playerDataDict[coinsStr]);
        playerData.prestige = float.Parse(playerDataDict[prestigeStr]);
        playerData.stars = int.Parse(playerDataDict[starsStr]);
        playerData.skinAvailability = int.Parse(playerDataDict[skinAvailabilityStr]);
    }   
}
