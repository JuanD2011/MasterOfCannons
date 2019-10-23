using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Settings settings = null;

    [SerializeField]
    private PlayerData playerData = null;

    public static bool canSelectLevel = false;

    private static bool languageSetOnce = false;

    protected SettingsTabManager settingsTabManager = null;

    public event Delegates.Action<int> OnLoadLevel = null;

    private void Awake()
    {
        settingsTabManager = GetComponent<SettingsTabManager>();

        Memento.LoadData(settings);

        if (!languageSetOnce)
        {
            SetLanguage();
            languageSetOnce = true;
        }

        CheckIfIsInLevelSelection();
    }

    private void Start()
    {
        Level.OnLevelSelected += ManageLevelPlayerAction;
    }

    private void CheckIfIsInLevelSelection()
    {
        if (!MenuGameManager.LevelSelection) return;

        settingsTabManager.PanelAnim(4);//Initialize level selection panel
        SelectingLevels(true);
    }

    private void ManageLevelPlayerAction(int _StarsNeeded, int _LevelBuildIndex)
    {
        if (playerData.stars >= _StarsNeeded)
        {
            settingsTabManager.PanelAnim(1);
            SendOnLoadLevel(_LevelBuildIndex);
        }
        else
        {
            Debug.Log("Insufficient Stars");
        }
    }

    private void SetLanguage()
    {
        Translation.currentLanguageId = settings.languageID;
        Translation.LoadLanguage(Translation.idToLanguage[Translation.currentLanguageId]);
    }

    private void OnApplicationQuit()
    {
        if (settings == null) return;

        Memento.SaveData(settings);
    }
    
    /// <summary>
    /// Equals can select level to the _value
    /// </summary>
    /// <param name="_Value"></param>
    private void SelectingLevels(bool _Value) => canSelectLevel = _Value;

    /// <summary>
    /// Save settings
    /// </summary>
    public void SaveSettings() => Memento.SaveData(settings);

    protected void SendOnLoadLevel(int _LevelBuildIndex) => OnLoadLevel(_LevelBuildIndex);
}
