using System.Collections.Generic;
using UnityEngine;

public class MovingCannon : Cannon
{
    [SerializeField]
    private bool startMoving = false;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private iTween.EaseType easeType;

    private List<Vector3> targets = new List<Vector3>();

    private byte targetCounter = 0;

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

    private void Move()
    {
        MoveToFirstTarget();
    }

    private void MoveToFirstTarget()
    {
        iTween.MoveTo(gameObject, iTween.Hash("position", targets[0], "easeType", easeType, "speed", speed, "oncomplete", "MoveToNextTarget"));
        Debug.Log("Moving to first target");
        Debug.Log("Target counter" + targetCounter);
    }

    private void MoveToNextTarget()
    {
        targetCounter++;
        if (targetCounter < targets.Count)
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", targets[targetCounter], "easeType", easeType, "speed", speed, "oncomplete", "MoveToFirstTarget")); 
        }
        else if (targetCounter == targets.Count)
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", targets[targetCounter], "easeType", easeType, "speed", speed, "oncomplete", "MoveToPreviousTarget"));
        }
    }

    private void MoveToPreviousTarget()
    {
        targetCounter--;
        if (targetCounter > 0)
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", targets[targetCounter], "easeType", easeType, "speed", speed, "oncomplete", "MoveToPreviousTarget"));
        }
        else if (targetCounter == 0)
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", targets[targetCounter], "easeType", easeType, "speed", speed, "oncomplete", "MoveToNextTarget"));
        }
    }
}
