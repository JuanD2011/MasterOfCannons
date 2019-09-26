using System;
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

    public static event Action<VolumeLevelStatusType> OnVolumeEntered = null;

    private void OnTriggerEnter(Collider _Other)
    {
        OnVolumeEntered?.Invoke(volumeType);
    }

    private void Reset()
    {
        GetComponent<Collider>().isTrigger = true;
    }
}
