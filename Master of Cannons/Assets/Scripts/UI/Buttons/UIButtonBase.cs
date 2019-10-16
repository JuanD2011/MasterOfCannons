using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class UIButtonBase : MonoBehaviour
{
    protected Button m_Button = null;

    protected virtual void Awake()
    {
        m_Button = GetComponent<Button>();
    }

    protected virtual
        void Start()
    {
        m_Button.onClick.AddListener(() => OnButtonClicked());
    }

    /// <summary>
    /// Execute method when the button has been pressed
    /// </summary>
    public abstract void OnButtonClicked();
}
