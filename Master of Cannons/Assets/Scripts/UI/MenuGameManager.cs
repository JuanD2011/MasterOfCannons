public class MenuGameManager : MenuManager
{
    public static event Delegates.Action<bool> OnPause = null;

    public static bool IsPaused { get; private set; } = false;

    private void Awake()
    {
        OnPause = null;

        settingsTabManager = GetComponent<SettingsTabManager>();
    }

    private void Start()
    {
        LevelManager.OnLoadLevel += (LoadLevelStatusType _LoadLevelStatusType) =>
        {
            if (_LoadLevelStatusType == LoadLevelStatusType.Successful)
            {
                settingsTabManager.PanelAnim(1);//Start Loading Screen
            }
        };
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
}
