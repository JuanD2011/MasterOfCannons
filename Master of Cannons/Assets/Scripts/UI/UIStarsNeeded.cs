using UnityEngine;
using TMPro;

public class UIStarsNeeded : MonoBehaviour
{
    [SerializeField] Level m_Level = null;
    TextMeshProUGUI textMeshProUGUI = null;

    private void Awake()
    {
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();

        textMeshProUGUI.text = m_Level.StarsNedeed.ToString();
    }
}
