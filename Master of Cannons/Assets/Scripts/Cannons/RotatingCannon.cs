using UnityEngine;

public class RotatingCannon : MovingCannon
{

    [Tooltip("Will find the shortest path to reach the angle")]
    [SerializeField] Vector3[] angles = new Vector3[2];

    int anglesCounter = 0;

    protected override void Start()
    {
        base.Start();
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

    }
}
