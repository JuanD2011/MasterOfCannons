using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    [SerializeField] private bool dontDestroyOnLoad = false;

    private static T instance = null;

    public static T Instance { get => instance; }

    private void Awake()
    {
        if (Instance == null)
        {
            instance = this as T;
        }
        else
        {
            if (dontDestroyOnLoad)
            {
                Destroy(gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        if (dontDestroyOnLoad)
        {
            DontDestroyOnLoad(gameObject);
        }

        OnAwake();
    }

    /// <summary>
    /// This should be called by the classes that needs to implement Awake method.
    /// </summary>
    protected abstract void OnAwake();
    
    /// <summary>
    /// Return true if the Instance is not null.
    /// </summary>
    /// <returns></returns>
    public static bool IsNotNull()
    {
        if (Instance != null) return true;
        return false;
    }
}
