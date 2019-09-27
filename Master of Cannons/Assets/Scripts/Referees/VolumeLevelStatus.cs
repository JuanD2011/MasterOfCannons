using UnityEngine;

public enum VolumeLevelStatusType
{
    Victory,
    Defeat,
    None
};

[RequireComponent(typeof(BoxCollider))]
public class VolumeLevelStatus : MonoBehaviour
{
    [SerializeField] VolumeLevelStatusType volumeType = VolumeLevelStatusType.None;

    public static event Delegates.Action<VolumeLevelStatusType> OnVolumeEntered = null;

    private void OnTriggerEnter(Collider _Other)
    {
        OnVolumeEntered(volumeType);
    }

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}
