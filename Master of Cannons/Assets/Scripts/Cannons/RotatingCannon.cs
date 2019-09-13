using UnityEngine;

public class RotatingCannon : MovingCannon
{
    [Tooltip("Will find the shortest path to reach the angle")]
    [SerializeField] Vector3[] angles = new Vector3[2];

    int anglesCounter = 0;

    protected override void Start()
    {
        repeatMethod += Rotate;
        if (startMoving) Move();
    }

    protected override void Update()
    {
        base.Update();
    }


    protected override void Move()
    {
        Rotate();
    }

    private void Rotate()
    {
        LeanTween.rotate(gameObject, angles[anglesCounter], 1f).setEase(tweenType).setOnComplete(repeatMethod).setSpeed(speed);
        anglesCounter = (anglesCounter + 1) % angles.Length;
    }
}
