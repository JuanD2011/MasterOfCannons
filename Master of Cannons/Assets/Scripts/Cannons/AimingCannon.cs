using UnityEngine;

public class AimingCannon : Cannon
{
    private float targetRotation = 0f;

    private Vector3 direction = Vector3.zero;

    private Camera m_Camera = null;

    private bool canAim = false;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        m_Camera = Camera.main;
    }

    protected override void Update()
    {
        base.Update();

        if (canAim && !MenuManager.IsPaused)
        {
            if (Input.GetMouseButton(0))
            {
                AimCannon(Input.mousePosition);
            } 
        }
    }

    private void AimCannon(Vector2 _AimVector)
    {
        direction = new Vector3(_AimVector.x, _AimVector.y, 0) - m_Camera.WorldToScreenPoint(transform.position);
        targetRotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(targetRotation - 90, Vector3.forward);
    }

    protected override void CatchCharacter()
    {
        canAim = true;
        base.CatchCharacter();
    }

    protected override void Shoot()
    {
        canAim = false;
        base.Shoot();
    }
}
