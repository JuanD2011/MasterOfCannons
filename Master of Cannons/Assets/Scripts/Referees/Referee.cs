using UnityEngine;

public class Referee : MonoBehaviour
{
    public static event Delegates.Action OnGameOver = null;

    private void Awake()
    {
        OnGameOver = null;
    }

    private void Start()
    {
        VolumeLevelStatus.OnVolumeEntered += ManageVolumeStatus;
    }

    private void ManageVolumeStatus(VolumeLevelStatusType _VolumeLevelStatus)
    {
        switch (_VolumeLevelStatus)
        {
            case VolumeLevelStatusType.Victory:
                Debug.Log("Victory");
                break;
            case VolumeLevelStatusType.Defeat:
                Debug.Log("Defeat");
                break;
            case VolumeLevelStatusType.None:
                break;
            default:
                break;
        }
        OnGameOver();
    }
}
