using UnityEngine;
using System.Collections.Generic;

public class MovingBehaviour : CannonBehaviour
{
    [SerializeField]
    protected bool startMoving = true;

    [SerializeField]
    protected float speed = 5f;

    [SerializeField]
    protected LeanTweenType tweenType = LeanTweenType.easeInOutSine;

    private List<Vector3> targets = new List<Vector3>();

    private int targetCounter = 0;

    protected System.Action repeatMethod = null;
    public List<Vector3> Targets { get => targets; }

    protected override void Awake()
    {
        base.Awake();
        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).CompareTag("Target")) targets.Add(transform.GetChild(i).position);
    }

    private void Start()
    {
        repeatMethod += Move;

        if (startMoving) Move();

        MenuGameManager.onPause += PauseMovement;
    }

    private void PauseMovement(bool _Value)
    {
        if (_Value) LeanTween.pauseAll();
        else LeanTween.resumeAll();
    }

    protected virtual void Move()
    {
        LeanTween.move(gameObject, targets[targetCounter], 1f).setEase(tweenType).setOnComplete(repeatMethod).setSpeed(speed);
        targetCounter = (targetCounter + 1) % targets.Count;
    }

    protected override void OnCharacterInCannon(bool _value)
    {
        if (!startMoving)
        {
            if (_value) Move();
            else PauseMovement(true);
        }
    }
}
