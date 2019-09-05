using UnityEngine;

public class MovingCannon : Cannon
{
    [SerializeField]
    private bool startMoving = false;

    [SerializeField]
    Transform target1 = null, target2 = null;

    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private iTween.EaseType easeType;

    private bool verticalCannon = false;

    private void Start()
    {
        //if (target1.position.y > transform.position.y || target2.position.y > transform.position.y || target1.position.y < transform.position.y || target2.position.y < transform.position.y) verticalCannon = true;
        //else verticalCannon = false;

        target1.SetParent(null);
        target2.SetParent(null);

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
        if (!verticalCannon)
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", target1.position, "easeType", easeType, "speed", speed, "oncomplete", "MoveToSecondTarget"));
        }
        else
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", target1.position, "easeType", easeType, "speed", speed, "oncomplete", "MoveToSecondTarget"));
        }
    }

    private void MoveToSecondTarget()
    {
        if (!verticalCannon)
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", target2.position, "easeType", easeType, "speed", speed, "oncomplete", "MoveToFirstTarget"));
        }
        else
        {
            iTween.MoveTo(gameObject, iTween.Hash("position", target2.position, "easeType", easeType, "speed", speed, "oncomplete", "MoveToFirstTarget"));
        }
    }
}
