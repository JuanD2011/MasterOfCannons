using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public abstract class Collectible : MonoBehaviour
{
    protected CollectibleType collectibleType = CollectibleType.None;

    public static event Delegates.Action<CollectibleType> onCollected = null;

    protected virtual void Awake()
    {
        onCollected = null;
    }

    private void OnTriggerEnter(Collider other) => Collect();

    protected virtual void Collect() => onCollected(collectibleType);

    private void Reset()
    {
        GetComponent<CapsuleCollider>().isTrigger = true;
        gameObject.layer = 13;//Collectible layer
    }
}
