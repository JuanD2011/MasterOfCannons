using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private TextAsset playerDataTextAsset = null;
    [SerializeField] private TextAsset playerLevelsDataTextAsset = null;

    [SerializeField] private Settings settings = null;

    [SerializeField] private PlayerData playerData = null;
    [SerializeField] private PlayerLevelsData playerLevelsData = null;
    [SerializeField] private GameData gameData = null;

    [SerializeField] private UIBackground uIBackground = null;

    public static bool canSelectLevel = false;
        
    private static bool languageSetOnce = false;
    private static bool settingsLoaded = false;
    private static bool playerDataLoaded = false;

    protected SettingsTabManager settingsTabManager = null;

    public event Delegates.Action<int> OnLoadLevel = null;

    private void Awake()
    {
        settingsTabManager = GetComponent<SettingsTabManager>();

        //Only set language when the user joins the app
        if (!languageSetOnce)
        {
            InitializeLanguage();
            languageSetOnce = true;
        }

        //Only load settings when user joins the app
        if (!settingsLoaded)
        {
            Memento.LoadData(settings);
            settingsLoaded = true;
        }

        //TODO load player data when he has logged in
        if (!playerDataLoaded)
        {
            Memento.LoadData(playerLevelsData, playerLevelsDataTextAsset.text);
            Memento.LoadData(playerData, playerDataTextAsset.text);
            playerDataLoaded = true;
        }
    }

    private void Start()
    {
        CheckIfIsInLevelSelection();//In start because we depend on settings tab manager

        Level.OnLevelSelected += ManageLevelPlayerAction;
    }

    private void OnDestroy()
    {
        Level.OnLevelSelected -= ManageLevelPlayerAction;
    }

    private void CheckIfIsInLevelSelection()
    {
        if (!MenuGameManager.LevelSelection) return;

        settingsTabManager.PanelAnim(4);//Initialize level selection panel
        uIBackground.SetActive(false);
        SelectingLevels(true);
    }

    /// <summary>
    /// Check if the player has sufficent stars, in that case, will load the scene
    /// </summary>
    /// <param name="_levelData"></param>
    private void ManageLevelPlayerAction(LevelData _levelData)
    {
        if (playerData.stars >= _levelData.starsNedeed)
        {
            settingsTabManager.PanelAnim(1);
            gameData.currentLevelData = _levelData;

            SendOnLoadLevel(_levelData.levelBuildIndex);
        }
        else
        {
            Debug.Log("Insufficient Stars");
        }
    }

    /// <summary>
    /// Set language
    /// </summary>
    private void InitializeLanguage()
    {
        Translation.CurrentLanguageId = settings.languageId;
        Translation.LoadLanguage(Translation.idToLanguage[Translation.CurrentLanguageId].ToString());
    }

    private void OnApplicationQuit()
    {
        if (settings == null) return;
#if !UNITY_EDITOR
        Memento.SaveData(settings);
#endif
    }
    
    /// <summary>
    /// Equals can select level to the _value
    /// </summary>
    /// <param name="_value"></param>
    private void SelectingLevels(bool _value) => canSelectLevel = _value;

    /// <summary>
    /// Save settings
    /// </summary>
    public void SaveSettings() => Memento.SaveData(settings);

    protected void SendOnLoadLevel(int _levelBuildIndex) => OnLoadLevel(_levelBuildIndex);
}
