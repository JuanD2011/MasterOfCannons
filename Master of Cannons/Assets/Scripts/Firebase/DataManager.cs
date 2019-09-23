using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager DM = null;
    public Settings settings;

    private void Awake()
    {
        //Memento.ClearData(settings);
        if (DM == null) DM = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        
    }

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(settings.defaultScene);
    }

}
