using UnityEngine;

public class Referee : MonoBehaviour
{
    [SerializeField]
    private Transform losingVolume = null;

    [SerializeField]
    private float offset = 0f;

    private void Awake()
    {
        VolumeLevelStatus.OnVolumeEntered -= ManageVolumeStatus;       
    }

    private void Start()
    {
        VolumeLevelStatus.OnVolumeEntered += ManageVolumeStatus;
        //losingColliderObj.transform.position
        //= cannonEntry.transform.position - posDifferenceFromCannon;
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
