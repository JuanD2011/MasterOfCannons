using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class LevelSelectionScroll : MonoBehaviour
{
    [SerializeField] Transform worldLevels = null;
    [SerializeField] float leftLimit = 0, rightLimit = 0;

    ScrollRect m_ScrollRect = null;

    Settings settings = null;

    private float m = 0f;

    private void Awake()
    {
        settings = Resources.Load<Settings>("Scriptable Objects/Settings");
        m_ScrollRect = GetComponent<ScrollRect>();

        m = -rightLimit - leftLimit;

        InitializePosition();
    }


    private void Update()
    {
        if (m_ScrollRect.velocity == Vector2.zero) return;
        settings.scrollPosition = m_ScrollRect.horizontalNormalizedPosition;
        SetPosition();
    }

    private void InitializePosition()
    {
        m_ScrollRect.horizontalNormalizedPosition = settings.scrollPosition;
        SetPosition();
    }

    private void SetPosition()
    {
        worldLevels.localPosition = new Vector3(m * settings.scrollPosition + leftLimit, 0, 0);   
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(leftLimit, 0f, 0f), new Vector3(rightLimit, 0f, 0f));
    }

    private void Reset()
    {
        GetComponent<ScrollRect>().vertical = false;
    }
}
