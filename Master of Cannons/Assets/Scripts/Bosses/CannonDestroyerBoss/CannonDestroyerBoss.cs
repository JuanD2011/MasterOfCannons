using UnityEngine;

public class CannonDestroyerBoss : Boss
{
    [SerializeField] private GameObject cannonsParent = null;

    [Header("Bullets")]
    [SerializeField] private float bulletLifeTime = 0f;

    [Header("Movement")]
    [SerializeField] private Collider moveVolume = null;
    [SerializeField] private Vector2 movementSpeedRange = Vector2.zero;
    [SerializeField] private LeanTweenType easeType = LeanTweenType.notUsed;
    private float speed = 0f;
    private Vector3 nextPosition = Vector3.zero;

    private Cannon[] cannons;

    private void Start()
    {
        cannons = cannonsParent.GetComponentsInChildren<Cannon>();
        Move();
    }

    private void Move()
    {
        speed = Random.Range(movementSpeedRange.x, movementSpeedRange.y);
        nextPosition = moveVolume.GetRandomPointInVolume();
        LeanTween.move(gameObject, nextPosition, 1f).setSpeed(speed).setEase(easeType).setOnComplete(MoveToNextPoint);
    }

    private void MoveToNextPoint() => Move();

    protected override void SetDifficulty()
    {

    }

    protected override void BossDamaged()
    {
        
    }
}
