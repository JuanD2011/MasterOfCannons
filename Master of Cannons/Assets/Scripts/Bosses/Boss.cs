using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    [SerializeField]
    protected float life = 0f;

    [SerializeField, Range(1, 10)]
    protected byte difficulty = 1;

    [SerializeField]
    protected Character character = null;

    protected virtual void Awake()
    {
        SetDifficulty();
    }

    protected abstract void SetDifficulty();
}
