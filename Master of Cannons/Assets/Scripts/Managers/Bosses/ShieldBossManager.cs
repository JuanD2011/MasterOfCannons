using UnityEngine;

public class ShieldBossManager : BossFightManager
{
    public static ShieldBossManager Instance;

    [SerializeField]
    private int damagePerHit = 0;

    [SerializeField]
    private float CharacterLerpSpeed = 0f;

    [SerializeField]
    private LeanTweenType characterLerpType = LeanTweenType.linear;

    private Vector3 characterStartPosition = Vector3.zero;

    public int DamagePerHit { get => damagePerHit; private set => damagePerHit = value; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        characterStartPosition = character.transform.position;
        Boss.OnBossDamage += OnBossDamage;
    }

    private void OnDestroy() => Boss.OnBossDamage -= OnBossDamage;

    protected override void OnBossDamage(int _bossLife)
    {
        print("called");
        character.SetFunctional(false);
        LeanTween.move(character.gameObject, characterStartPosition, 1f)
            .setSpeed(CharacterLerpSpeed)
            .setEase(characterLerpType)
            .setOnComplete(CharacterLerpCompleted);
        Debug.Log(_bossLife);
    }

    private void CharacterLerpCompleted() => character.SetFunctional(true);
}
