using UnityEngine;
using TMPro;
using Delegates;
using System.Collections;

public class UIPlayerData : MonoBehaviour
{
    [Header("UI Player Data")]
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI xp;
    public static Action<string, string, string> showPlayerData;

    public static Action<string> showCoins;
    private IEnumerator Start()
    {
        showCoins = (coins) => { this.coins.text = coins; };
        showPlayerData = (username, coins, xp) => { this.username.text = string.Format("Username: {0}", username);
                                                    this.coins.text = coins;
                                                    this.xp.text = xp; ; };

        yield return new WaitUntil(() => FirebaseAuthManager.myUser != null && FirebaseAuthManager.CheckDependenciesHandler());
        //yield return new WaitUntil(() => FirebaseAuthManager.updateProfileTask?.IsCompleted == true && FirebaseAuthManager.myUser.DisplayName != string.Empty);
        FirebaseDBManager.DB.GetPlayerData(showPlayerData);
    }
   
}
