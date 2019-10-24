using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    [SerializeField]
    private int life = 0;

    [SerializeField, Range(1, 10)]
    protected byte difficulty = 1;

    protected int Life { get => life; private set => life = value; }

    public static event Delegates.Action<int> OnBossHit = null;

    protected virtual void Awake()
    {
        SetDifficulty();
        OnBossHit = null;
    }

    protected abstract void SetDifficulty();

    protected void BossHit()
    {
        Life -= BossFightManager.BossFight.DamagePerHit;
        OnBossHit?.Invoke(Life);
    } 
}
