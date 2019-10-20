using UnityEngine;

public class UIWorldToScreen : MonoBehaviour
{
    [SerializeField] Camera m_Camera = null;
    [SerializeField] Transform target = null;
    [SerializeField] Vector3 offset = Vector3.zero;

    private void Update()
    {
        transform.position = m_Camera.WorldToScreenPoint(target.position + offset);
    }
}
