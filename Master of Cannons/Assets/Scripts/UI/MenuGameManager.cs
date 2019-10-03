public class MenuGameManager : MenuManager
{
    public static event Delegates.Action<bool> OnPause = null;

    public static bool IsPaused { get; private set; } = false;

    protected override void Awake()
    {
        OnPause = null;

        VolumeLevelStatus.OnVolumeEntered -= ManageVolumeStatus;

        InitializePanelAnimators();
    }

    protected override void Start()
    {
        base.Start();
        VolumeLevelStatus.OnVolumeEntered += ManageVolumeStatus;
    }

    private void ManageVolumeStatus(VolumeLevelStatusType _VolumeStatus)
    {
        switch (_VolumeStatus)
        {
            case VolumeLevelStatusType.Victory:
                PanelAnim(2);
                break;
            case VolumeLevelStatusType.Defeat:
                PanelAnim(3);
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
        OnPause?.Invoke(_Value);
    }
}
