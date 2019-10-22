using UnityEngine;

public class RotatingBehaviour : MovingBehaviour
{
    [Tooltip("Will find the shortest path to reach the angle")]
    [SerializeField] Vector3[] angles = new Vector3[2];

    private Vector3 initialRotation = Vector3.zero;

    int anglesCounter = 0;

    protected override void Awake() => base.Awake();

    private void Start()
    {
        initialRotation = transform.eulerAngles;
        repeatMethod += Move;
        cannon.OnCharacterInCannon += OnCharacterInCannon;

        if (startMoving) Move();
    }

    protected override void Move()
    {
        LeanTween.rotate(gameObject, angles[anglesCounter] + initialRotation, 1f).setEase(tweenType).setOnComplete(repeatMethod).setSpeed(speed);
        anglesCounter = (anglesCounter + 1) % angles.Length;
    }

    protected override void OnCharacterInCannon(bool _value) => base.OnCharacterInCannon(_value);
}
