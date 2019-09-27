using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager DM = null;
    public Settings settings;
    [SerializeField] bool clearData;

    private void Awake()
    {
        //if(clearData) Memento.ClearData(settings);
        if (DM == null) DM = this;
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
        Memento.LoadData(settings);
    }

}
