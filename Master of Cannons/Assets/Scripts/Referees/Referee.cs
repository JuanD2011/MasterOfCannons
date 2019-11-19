using UnityEngine;

public class Referee : MonoBehaviour
{
    public static event Delegates.Action OnGameOver = null;
    [SerializeField] private GameObject losingVolumeCompound = null;

    private void Awake() => OnGameOver = null;

    private void Start()
    {
        VolumeLevelStatus.OnVolumeEntered += ManageVolumeStatus;
        Cannon.OnChangeLosingBoundaries += ChangeLosingBoundaries;
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

    private void ChangeLosingBoundaries(Vector3 _latestCannonHit)
    {
        Vector3 posToChange = losingVolumeCompound.transform.position;
        posToChange.y = _latestCannonHit.y;
        losingVolumeCompound.transform.position = posToChange;
    }
}
