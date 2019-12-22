using UnityEngine;
using TMPro;

public class UILevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI = null;

    private PlayerData playerData = null;//TODO load this from other script, to avoid loadinging constantly the class

    private void Awake()
    {
        playerData = Resources.Load<PlayerData>("Scriptable Objects/Player Data");//TODO Load this from firebase
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
            GetComponent<CanvasGroup>().alpha = 0;
        }
    }
}
