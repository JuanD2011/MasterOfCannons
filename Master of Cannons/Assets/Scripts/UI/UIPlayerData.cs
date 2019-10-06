using UnityEngine;
using TMPro;
using Delegates;
using System.Collections;

public class UIPlayerData : MonoBehaviour
{
    [Header("UI Player Data")]
    [SerializeField] TextMeshProUGUI username = null;
    [SerializeField] TextMeshProUGUI coins = null;
    [SerializeField] TextMeshProUGUI prestige = null;
    public static Action<string, string, string> showPlayerData;

    public static Action<string> showCoins;
    private IEnumerator Start()
    {
        showCoins = (coins) => { this.coins.text = coins; };
        showPlayerData = (username, coins, prestige) => { this.username.text = string.Format("Username: {0}", username);
                                                    this.coins.text = coins;
                                                    this.prestige.text = prestige; ; };

        yield return new WaitUntil(() => FirebaseAuthManager.myUser != null && FirebaseAuthManager.CheckDependenciesHandler());
        FirebaseDBManager.DB.GetPlayerData(showPlayerData);
    }
   
}
