using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    [SerializeField] private bool dontDestroyOnLoad = false;

    public static T Instance { get; protected set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
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
   
}
