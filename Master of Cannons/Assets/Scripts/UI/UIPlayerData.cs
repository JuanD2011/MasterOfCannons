using UnityEngine;
using TMPro;
using Delegates;
using System.Collections;
using UnityEngine.UI;

public class UIPlayerData : MonoBehaviour
{
    [Header("UI Player Data")]
    [SerializeField] TextMeshProUGUI username = null;
    [SerializeField] TextMeshProUGUI coins = null;
    [SerializeField] TextMeshProUGUI prestige = null;
    public static Action<string, string, string> showPlayerData;

    [SerializeField] TextMeshProUGUI coinChestTime = null;
    [SerializeField] Button coinChestButt = null;

    public static Action<string> showCoins;
    public static Action<string> showChestsInfo;
    public static Action updateChestsInfo;
    public static Action<int> updateCoins;
    
    private IEnumerator Start()
    {
        coinChestButt = coinChestTime.GetComponentInParent<Button>();
        coinChestButt.onClick.AddListener(()=> {
            FirebaseDBManager.DB.UpdateCoinsGiftInfo(() => DailyGiftManager.getGiftTimesHandler());
            DailyGiftManager.giveRewardHandler.Invoke();
            coinChestButt.interactable = false;            
            });


        updateChestsInfo = UpdateChestButton;
        showChestsInfo = ShowChestsInfo;
        updateCoins = UpdateCoins;

        showCoins = (coins) => { this.coins.text = coins; };
        showPlayerData = (username, coins, prestige) => { this.username.text = string.Format("Username: {0}", username);
                                                    this.coins.text = coins;
                                                    this.prestige.text = prestige; ; };

        yield return new WaitUntil(() => FirebaseAuthManager.myUser != null && FirebaseAuthManager.CheckDependenciesHandler());
        FirebaseDBManager.DB.GetPlayerData(showPlayerData);
    }
   
    void ShowChestsInfo(string _coinChestTime)
    {
        coinChestTime.text = _coinChestTime;
    }

    void UpdateChestButton()
    {
        coinChestTime.text = "Open";
        coinChestButt.interactable = true;
    }

    public void UpdateCoins(int _coins)
    {
        DataManager.DM.playerData.coins += _coins;
        showCoins(DataManager.DM.playerData.coins.ToString());
        FirebaseDBManager.DB.WriteNewCoins(DataManager.DM.playerData.coins);
    }
}
