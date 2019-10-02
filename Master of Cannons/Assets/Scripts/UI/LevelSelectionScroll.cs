using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionScroll : MonoBehaviour
{
    [SerializeField] Transform worldLevels = null;
    [SerializeField] float leftLimit = 0, rightLimit = 0;

    ScrollRect m_ScrollRect = null;

    float m = 0f;

    private void Awake()
    {
        m_ScrollRect = GetComponent<ScrollRect>();
    }

    private void Start()
    {
        m = -rightLimit - leftLimit;
    }

    private void Update()
    {
        if (m_ScrollRect.velocity == Vector2.zero) return;
        worldLevels.localPosition = new Vector3(m * m_ScrollRect.horizontalNormalizedPosition + leftLimit, 0, 0);   
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(leftLimit, 0f, 0f), new Vector3(rightLimit, 0f, 0f));
    }
}
