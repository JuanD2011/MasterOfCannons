using UnityEngine;

public class RotatingCannon : MovingCannon
{
    [Tooltip("Will find the shortest path to reach the angle")]
    [SerializeField] Vector3[] angles = new Vector3[2];

    int anglesCounter = 0;

    private void Start()
    {
        repeatMethod += Move;

        if (startMoving) Move();
    }

    protected override void Move()
    {
        LeanTween.rotate(gameObject, angles[anglesCounter], 1f).setEase(tweenType).setOnComplete(repeatMethod).setSpeed(speed);
        anglesCounter = (anglesCounter + 1) % angles.Length;
    }
}
