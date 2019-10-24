using UnityEngine;

public class RotatingShieldBoss : Boss
{
    private Vector2 rotationSpeedRange = new Vector2(80f, 100f);
    private float timeToRotate = 2f, timeInState = 0f;
    Vector3 rotation = new Vector3();
    private RotatingShieldBossState state = RotatingShieldBossState.None;
    private System.Action OnCompleteAction = null;

    [SerializeField]
    private LeanTweenType tweenType = LeanTweenType.linear;

    protected override void Awake() => base.Awake();

    private void Start()
    {
        SwitchState(RotatingShieldBossState.Chase);
    }

    /// <summary>
    /// Switch to Rotating Shield Boss states
    /// </summary>
    /// <param name="stateToSwitchTo"></param>
    public void SwitchState(RotatingShieldBossState _state)
    {
        if (_state != RotatingShieldBossState.None)
        {
            StopAllCoroutines();
            if (_state == RotatingShieldBossState.FreeMovement) FreeMovement();
            else Chase(); 
        }
    }

    private void SwitchNextState()
    {
        Chase();
        //if (state == RotatingShieldBossState.FreeMovement) Chase();
        //else FreeMovement();
    }

    private void Chase()
    {
        Debug.Log("Chase");
        state = RotatingShieldBossState.Chase;
        OnCompleteAction = SwitchNextState;
        Vector3 target = Quaternion.LookRotation(character.transform.position - transform.position, transform.forward).eulerAngles;
        rotation = new Vector3(target.x, target.y, 90);

        LeanTween.rotate(gameObject, rotation, timeToRotate)
            .setEase(tweenType)
            .setOnComplete(OnCompleteAction);
    }

    private void FreeMovement()
    {
        //state = RotatingShieldBossState.FreeMovement;
        //rotation = new Quaternion(transform.rotation.x, Random.Range(0f, 360f), transform.rotation.z, transform.rotation.w).eulerAngles;
        //speed = Random.Range(rotationSpeedRange[0], rotationSpeedRange[1]);

        //LeanTween.rotateZ(gameObject, rotation.y, 0f)
        //    .setSpeed(speed)
        //    .setOnComplete(OnCompleteAction)
        //    .setEase(tweenType);
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
                rotationSpeedRange *= 1.2f;
                timeInState *= 0.9f;
                break;
            case 3:
                timeToRotate *= 1.0f;
                rotationSpeedRange *= 1.4f;
                timeInState *= 0.8f;
                break;
            case 4:
                timeToRotate *= 0.9f;
                rotationSpeedRange *= 1.6f;
                timeInState *= 0.7f;
                break;
            case 5:
                timeToRotate *= 0.8f;
                rotationSpeedRange *= 1.8f;
                timeInState *= 0.6f;
                break;
            case 6:
                timeToRotate *= 0.7f;
                rotationSpeedRange *= 2.0f;
                timeInState *= 0.5f;
                break;
            case 7:
                timeToRotate *= 0.6f;
                rotationSpeedRange *= 2.2f;
                timeInState *= 0.4f;
                break;
            case 8:
                timeToRotate *= 0.5f;
                rotationSpeedRange *= 2.4f;
                timeInState *= 0.3f;
                break;
            case 9:
                timeToRotate *= 0.4f;
                rotationSpeedRange *= 2.6f;
                timeInState *= 0.2f;
                break;
            case 10:
                timeToRotate *= 0.3f;
                rotationSpeedRange *= 2.8f;
                timeInState *= 0.1f;
                break;
            default:
                break;
        }
    }
}

public enum RotatingShieldBossState
{
    FreeMovement,
    Chase,
    None
}