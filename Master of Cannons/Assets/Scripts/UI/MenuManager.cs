using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private Settings settings = null;

    public static bool canSelectLevel = false;

    protected SettingsTabManager settingsTabManager = null;

    private void Awake()
    {
        settingsTabManager = GetComponent<SettingsTabManager>();

        Memento.LoadData(settings);

        SetLanguage();

        CheckIfIsInLevelSelection();
    }

    private void Start()
    {
        LevelManager.OnLoadLevel += ManageLevelPlayerAction;
    }

    private void CheckIfIsInLevelSelection()
    {
        settingsTabManager.PanelAnim(4);//Initialize level selection panel
        SelectingLevels(true);
    }

    private void ManageLevelPlayerAction(LoadLevelStatusType _LoadLevelStatusType)
    {
        if (_LoadLevelStatusType == LoadLevelStatusType.Successful)
        {
            settingsTabManager.PanelAnim(1);
        }
        else if (_LoadLevelStatusType == LoadLevelStatusType.InsufficientStars)
        {
            //TODO show message that the player has not enough stars to play
        }
    }

    private void SetLanguage()
    {
        Translation.currentLanguageId = settings.languageID;
        Translation.LoadLanguage(Translation.idToLanguage[Translation.currentLanguageId]);
    }

    /// <summary>
    /// Equals can select level to the _value
    /// </summary>
    /// <param name="_Value"></param>
    public void SelectingLevels(bool _Value) => canSelectLevel = _Value;

    /// <summary>
    /// Save settings
    /// </summary>
    public void SaveSettings()
    {
        Memento.SaveData(settings);
    }

    private void OnApplicationQuit()
    {
        if (settings == null) return;

        Memento.SaveData(settings);
    }
}
