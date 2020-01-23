using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class VolumeLevelStatus : MonoBehaviour
{
    [SerializeField] LevelStatus volumeType = LevelStatus.None;

    public static event Delegates.Action<LevelStatus> OnVolumeEntered = null;

    private void OnTriggerEnter(Collider _Other) => OnVolumeEntered(volumeType);

    private void Reset() => GetComponent<Collider>().isTrigger = true;
}
