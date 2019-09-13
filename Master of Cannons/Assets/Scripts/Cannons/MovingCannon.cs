using UnityEngine;
using System.Collections.Generic;

public class MovingCannon : Cannon
{
    [SerializeField]
    protected bool startMoving = false;

    [SerializeField]
    protected float speed = 5f;

    private List<Vector3> targets = new List<Vector3>();

    private int targetCounter = 0;

    private string moveToTargetMethod = "MoveToTarget";

    protected override void Start()
    {
        base.Start();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Target")) targets.Add(transform.GetChild(i).position);
        }
        //if (startMoving) Move();
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
        targetCounter = (targetCounter + 1) % targets.Count;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
