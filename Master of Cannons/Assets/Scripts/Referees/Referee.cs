using UnityEngine;

public class Referee : MonoBehaviour
{
    private void Awake()
    {
        VolumeLevelStatus.OnVolumeEntered -= ManageVolumeStatus;       
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
    }
}
