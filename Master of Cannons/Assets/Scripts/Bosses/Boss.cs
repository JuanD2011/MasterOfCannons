using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    [SerializeField]
    private int life = 0;

    [SerializeField, Range(1, 10)]
    protected byte difficulty = 1;

    protected int Life { get => life; set => life = value; }

    public static event Delegates.Action<int> OnBossDamage = null;

    protected virtual void Awake()
    {
        SetDifficulty();
        OnBossDamage = null;
    }

    protected abstract void SetDifficulty();

    protected abstract void BossDamaged();

    protected void InvokeOnBossDamage(int _life) => OnBossDamage?.Invoke(_life);
}
