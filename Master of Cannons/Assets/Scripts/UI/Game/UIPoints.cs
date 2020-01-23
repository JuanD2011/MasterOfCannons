using UnityEngine;
using TMPro;

public class UIPoints : MonoBehaviour
{
    private TextMeshProUGUI m_Text = null;

    private void Awake()
    {
        m_Text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        ScoreManager.Instance.OnPointsUpdated += UpdateText;
    }

    private void UpdateText(int _points)
    {
        m_Text.SetText(_points.ToString());
    }
}
