using UnityEngine;

public class Referee : MonoBehaviour
{
    [SerializeField] private GameObject losingVolumeCompound = null;

    public static event Delegates.Action<LevelStatus> onGameOver = null;

    private void Awake()
    {
        onGameOver = null;
    } 

    private void Start()
    {
        VolumeLevelStatus.onVolumeEntered += ManageLevelStatus;
        Cannon.OnChangeLosingBoundaries += ChangeLosingBoundaries;
    }

    private void OnDestroy()
    {
        VolumeLevelStatus.onVolumeEntered -= ManageLevelStatus;
        Cannon.OnChangeLosingBoundaries -= ChangeLosingBoundaries;
    }

    /// <summary>
    /// Send game over event
    /// </summary>
    /// <param name="_levelStatus"></param>
    private void ManageLevelStatus(LevelStatus _levelStatus)
    {
        onGameOver(_levelStatus);
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
