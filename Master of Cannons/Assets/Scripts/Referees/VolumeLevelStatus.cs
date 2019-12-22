using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class VolumeLevelStatus : MonoBehaviour
{
    [SerializeField] LevelStatus volumeType = LevelStatus.None;

    public static event Delegates.Action<LevelStatus> onVolumeEntered = null;

    private void OnTriggerEnter(Collider _Other) => onVolumeEntered(volumeType);

    private void Reset() => GetComponent<Collider>().isTrigger = true;
}
