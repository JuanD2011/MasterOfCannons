using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UILevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI = null;

    [SerializeField] Image[] starsOn = new Image[3];

    private PlayerData playerData = null;//TODO load this from other script, to avoid constantly loading the class
    private PlayerLevelsData playerLevelsData = null;

    private void Awake()
    {
        playerData = Resources.Load<PlayerData>("Scriptable Objects/Player Data");//TODO Load this from firebase
        playerLevelsData = Resources.Load<PlayerLevelsData>("Scriptable Objects/Player Levels Data");//TODO Load this from firebase
    }

    /// <summary>
    /// Initialize UI information, stars for now
    /// </summary>
    /// <param name="_levelData"></param>
    public void Initialize(LevelData _levelData)
    {
        int starsCount = _levelData.starsNedeed - playerData.stars;

        if (starsCount > 0)
        {
            textMeshProUGUI.SetText(starsCount.ToString());
        }
        else
        {
            transform.GetChild(0).gameObject.SetActive(false);

            transform.GetChild(1).gameObject.SetActive(true);

            for (byte i = 0; i < playerLevelsData.levelsStars[_levelData.number - 1].stars; i++)
            {
                starsOn[i].enabled = true;
            }
        }
    }
}
