using UnityEngine.SceneManagement;

public class MenuGameManager : MenuManager
{
    public static event Delegates.Action<bool> OnPause = null;

    public static bool IsPaused { get; private set; } = false;
    public static bool LevelSelection { get; private set; } = false;

    private void Awake()
    {
        OnPause = null;

        settingsTabManager = GetComponent<SettingsTabManager>();
        LevelSelection = false;
    }

    private void Start()
    {
        VolumeLevelStatus.OnVolumeEntered += ManageVolumeStatus;
    }

    private void ManageVolumeStatus(VolumeLevelStatusType _VolumeStatus)
    {
        switch (_VolumeStatus)
        {
            case VolumeLevelStatusType.Victory:
                settingsTabManager.PanelAnim(3);
                break;
            case VolumeLevelStatusType.Defeat:
                settingsTabManager.PanelAnim(4);
                break;
            case VolumeLevelStatusType.None:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Set if the game is paused by the _Value given
    /// </summary>
    /// <param name="_Value"></param>
    public void SetGamePause(bool _Value)
    {
        IsPaused = _Value;
        OnPause.Invoke(_Value);
    }

    /// <summary>
    /// Restart current level by buildIndex
    /// </summary>
    public void RestartLevel()
    {
        settingsTabManager.PanelAnim(1);
        SendOnLoadLevel(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Load menu and set level selection panel
    /// </summary>
    public void LoadLevelSelection()
    {
        settingsTabManager.PanelAnim(1);
        LevelSelection = true;
        SendOnLoadLevel(0);
    }

    /// <summary>
    /// Load next level 
    /// </summary>
    public void NextLevel()
    {
        settingsTabManager.PanelAnim(1);
        SendOnLoadLevel(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
