using UnityEngine;

public class AimingCannon : Cannon
{
    float targetRotation = 0f;
    float turnSmoothVel = 0f, turnSmooth = 0.15f;

    Vector3 direction = Vector3.zero;

    Camera m_Camera = null;

    protected override void Awake()
    {
        base.Awake();
        PlayerInputHandler.OnAimAction -= AimCannon;
    }

    protected override void Start()
    {
        base.Start();
        PlayerInputHandler.OnAimAction += AimCannon;
        m_Camera = Camera.main;
    }

    private void AimCannon(Vector2 _AimVector)
    {
        direction = new Vector3(_AimVector.x, _AimVector.y, 0) - m_Camera.WorldToScreenPoint(transform.position);
        targetRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(targetRotation - 90, Vector3.forward);
    }
}
