using UnityEngine;

public class RotatingCannon : MovingCannon
{
    [SerializeField] iTween.LoopType loopType = iTween.LoopType.none;

    [Tooltip("Will find the shortest path to reach the angle")]
    [SerializeField] Vector3[] angles = new Vector3[2];

    int anglesCounter = 0;

    void Start()
    {
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
        iTween.RotateTo(gameObject, iTween.Hash("rotation", angles[anglesCounter], "easeType", easeType, "speed", speed, "ignoretimescale", true, "looptype", loopType, "oncomplete", "Rotate"));
        anglesCounter = (anglesCounter + 1) % angles.Length;
    }
}
