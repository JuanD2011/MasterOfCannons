using UnityEngine;

public class Referee : MonoBehaviour
{
    [SerializeField] private GameObject losingVolumeCompound = null;

    public static event Delegates.Action<LevelStatus> OnGameOver = null;

    private void Awake()
    {
        OnGameOver = null;
    } 

    private void Start()
    {
        VolumeLevelStatus.OnVolumeEntered += ManageLevelStatus;
        Cannon.OnChangeLosingBoundaries += ChangeLosingBoundaries;
    }

    private void OnDestroy()
    {
        VolumeLevelStatus.OnVolumeEntered -= ManageLevelStatus;
        Cannon.OnChangeLosingBoundaries -= ChangeLosingBoundaries;
    }

    /// <summary>
    /// Send game over event
    /// </summary>
    /// <param name="_levelStatus"></param>
    private void ManageLevelStatus(LevelStatus _levelStatus)
    {
        OnGameOver(_levelStatus);
    }

    /// <summary>
    /// Update losing volume, depending on the last cannon
    /// </summary>
    /// <param name="_latestCannonHit"></param>
    private void ChangeLosingBoundaries(Vector3 _latestCannonHit)
    {
        Vector3 posToChange = losingVolumeCompound.transform.position;
        posToChange.y = _latestCannonHit.y;
        losingVolumeCompound.transform.position = posToChange;
    }
}
