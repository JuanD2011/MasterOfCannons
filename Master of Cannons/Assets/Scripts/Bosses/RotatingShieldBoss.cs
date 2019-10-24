using UnityEngine;

public class RotatingShieldBoss : Boss
{
    private float timeToRotate = 2f;
    Vector3 rotation = new Vector3();
    private System.Action OnCompleteAction = null;

    [SerializeField]
    private LeanTweenType tweenType = LeanTweenType.linear;

    protected override void Awake() => base.Awake();

    private void Start()
    {
        Chase();
    }

    private void Chase()
    {
        OnCompleteAction = Chase;
        Vector3 target = Quaternion.LookRotation(BossFightManager.BossFight.Character.transform.position - transform.position, transform.forward).eulerAngles;
        rotation = new Vector3(target.x, target.y, 90);

        LeanTween.rotate(gameObject, rotation, timeToRotate)
            .setEase(tweenType)
            .setOnComplete(OnCompleteAction);
    }

    protected override void SetDifficulty()
    {
        switch (difficulty)
        {
            case 1:
                timeToRotate *= 1.2f;
                break;
            case 2:
                timeToRotate *= 1.1f;
                break;
            case 3:
                timeToRotate *= 1.0f;
                break;
            case 4:
                timeToRotate *= 0.9f;
                break;
            case 5:
                timeToRotate *= 0.8f;
                break;
            case 6:
                timeToRotate *= 0.7f;
                break;
            case 7:
                timeToRotate *= 0.6f;
                break;
            case 8:
                timeToRotate *= 0.5f;
                break;
            case 9:
                timeToRotate *= 0.4f;
                break;
            case 10:
                timeToRotate *= 0.3f;
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            BossHit();
        }
    }
}