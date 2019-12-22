using UnityEngine.SceneManagement;

public class MenuGameManager : MenuManager
{
    public static event Delegates.Action<bool> onPause = null;

    public static bool IsPaused { get; private set; } = false;
    public static bool LevelSelection { get; private set; } = false;

    private void Awake()
    {
        onPause = null;

        settingsTabManager = GetComponent<SettingsTabManager>();
        LevelSelection = false;
    }

    private void Start()
    {
        Referee.onGameOver += ManageRefereeAction;
    }

    private void OnDestroy()
    {
        Referee.onGameOver -= ManageRefereeAction;
    }

    private void ManageRefereeAction(LevelStatus _VolumeStatus)
    {
        switch (_VolumeStatus)
        {
            case LevelStatus.Victory:
                settingsTabManager.PanelAnim(3);
                break;
            case LevelStatus.Defeat:
                settingsTabManager.PanelAnim(4);
                break;
            case LevelStatus.None:
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
        onPause.Invoke(_Value);
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
