using UnityEngine;

public abstract class BossFightManager : MonoBehaviour
{
    [SerializeField]
    protected Character character = null;

    public Character Character { get => character; private set => character = value; }

    protected abstract void OnBossDamage(int _bossLife);
}
