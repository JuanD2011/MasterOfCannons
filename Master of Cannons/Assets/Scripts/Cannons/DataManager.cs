using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager DM = null;
    public Settings settings;
    [SerializeField] bool clearData;

    public Dictionary<string, string> playerData;
    public float coins, prestige, skinAvailability;

    public string coinsStr = "coins";
    public string prestigeStr = "prestige";
    public string skinAvailabilityStr = "skinAvailability";


    private void Awake()
    {
        //if(clearData) Memento.ClearData(settings);
        if (DM == null) DM = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        Memento.LoadData(settings);
    }

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => FirebaseAuthManager.myUser != null && FirebaseAuthManager.CheckDependenciesHandler());
        FirebaseDBManager.DB.GetPlayerData(UIPlayerData.showPlayerData);
    }

    public void InitializePlayerData(Dictionary<string,string> playerData)
    {
        coins = float.Parse(playerData[coinsStr]);
        prestige = float.Parse(playerData[prestigeStr]);
        skinAvailability = float.Parse(playerData[skinAvailabilityStr]);
    }
    
}
