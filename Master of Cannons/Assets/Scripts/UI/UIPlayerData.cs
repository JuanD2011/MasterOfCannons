using UnityEngine;
using TMPro;
using Delegates;

public class UIPlayerData : MonoBehaviour
{
    [Header("UI Player Data")]
    [SerializeField] TextMeshProUGUI username;
    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI xp;
    public static Action<string, string, string> showPlayerData;

    private void Start()
    {
        showPlayerData = (username, coins, xp) => { this.username.text = username;
                                                    this.coins.text = coins;
                                                    this.xp.text = xp; };

    }
   
}
