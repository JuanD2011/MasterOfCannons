using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BaseCanvas : MonoBehaviour
{
    ScriptableObject settings = null;
    Animator m_Animator = null;

    public static event System.Action OnSetStars = null;

    public static bool IsPaused { get; private set; } = false;

    private void Awake()
    {
        settings = Resources.Load<ScriptableObject>("Scriptable Objects/Settings");
        m_Animator = GetComponent<Animator>();
        //Memento.LoadData(settings);
    }

    /// <summary>
    /// Change the bool state of the _id parameter given, if it is false, it will be changed to true and the same to the contrary
    /// </summary>
    /// <param name="_id"></param>
    public void SetBool(string _id)
    {
        bool result = m_Animator.GetBool(_id);
        result = (!result) ? true : false;
        m_Animator.SetBool(_id, result);

        if (_id == "Configuration") IsPaused = result;
    }

    /// <summary>
    /// Save Settings which is in scriptable object folder
    /// </summary>
    public void SaveSettings()
    {
        Memento.SaveData(settings);
    }
}
