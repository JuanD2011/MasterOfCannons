using UnityEngine;

public class BossFightManager : MonoBehaviour
{
    public static BossFightManager BossFight;

    [SerializeField] GameObject characterGO = null;
    private Character character = null;

    [SerializeField]
    private int damagePerHit = 0;

    [SerializeField]
    private float CharacterLerpSpeed = 0f;

    [SerializeField]
    private LeanTweenType characterLerpType = LeanTweenType.linear;

    private Vector3 characterStartPosition = Vector3.zero;

    public Character Character { get => character; private set => character = value; }
    public int DamagePerHit { get => damagePerHit; private set => damagePerHit = value; }

    private void Awake()
    {
        if (BossFight == null) BossFight = this;
        else Destroy(this);
    }

    private void Start()
    {
        character = characterGO.GetComponent<Character>();
        characterStartPosition = character.transform.position;
        Boss.OnBossHit += OnBossHit;
    }

    private void OnBossHit(int _bossLife)
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
