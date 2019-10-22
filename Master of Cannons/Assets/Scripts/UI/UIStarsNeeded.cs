using UnityEngine;
using TMPro;

public class UIStarsNeeded : MonoBehaviour
{
    [SerializeField] private Level m_Level = null;
    [SerializeField] private TextMeshProUGUI textMeshProUGUI = null;

    private PlayerData playerData = null;

    private int starsCount = 0;

    private void Awake()
    {
        playerData = Resources.Load<PlayerData>("Scriptable Objects/Player Data");

        starsCount = m_Level.StarsNedeed - playerData.stars;

        if (starsCount > 0)
        {
            textMeshProUGUI.text = starsCount.ToString();
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = 0;
        }
    }
}
