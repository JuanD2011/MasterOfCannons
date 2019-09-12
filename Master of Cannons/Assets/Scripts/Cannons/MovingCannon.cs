using System.Collections.Generic;
using UnityEngine;

public class MovingCannon : Cannon
{
    [SerializeField]
    protected bool startMoving = false;

    [SerializeField]
    protected float speed = 5f;

    [SerializeField]
    protected iTween.EaseType easeType;

    private List<Vector3> targets = new List<Vector3>();

    private int targetCounter = 0;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            targets.Add(transform.GetChild(i).position);
        }
        Debug.Log(targets.Count);
        if (startMoving) Move();
    }

    protected override void Update()
    {
        base.Update();
    }

    protected virtual void Move()
    {
        MoveToTarget();
    }

    private void MoveToTarget()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", targets[targetCounter], "easeType", easeType, "speed", speed, "oncomplete", "MoveToTarget"));
        targetCounter = (targetCounter + 1) % targets.Count;
    }

}
