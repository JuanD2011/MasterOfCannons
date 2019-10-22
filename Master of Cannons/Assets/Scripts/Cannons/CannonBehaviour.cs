using UnityEngine;

public abstract class CannonBehaviour : MonoBehaviour
{
    protected Cannon cannon = null;

    protected virtual void Awake()
    {
        cannon = GetComponentInChildren<Cannon>();
    }

    protected abstract void OnCharacterInCannon(bool _value);
}
