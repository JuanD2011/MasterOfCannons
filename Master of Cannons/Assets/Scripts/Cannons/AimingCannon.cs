using UnityEngine;

public class AimingCannon : Cannon
{
    float targetRotation = 0f;
    float turnSmoothVel = 0f, turnSmooth = 0.15f;

    protected override void Awake()
    {
        base.Awake();
        PlayerInputHandler.OnAimAction -= AimCannon;
    }

    protected override void Start()
    {
        base.Start();
        PlayerInputHandler.OnAimAction += AimCannon;
    }

    private void AimCannon(Vector2 _AimVector)
    {
        if (_AimVector != Vector2.zero)
        {
            targetRotation = Mathf.Atan2(-_AimVector.x, _AimVector.y) * Mathf.Rad2Deg;
            transform.eulerAngles = Vector3.forward * Mathf.SmoothDampAngle(transform.eulerAngles.z, targetRotation, ref turnSmoothVel, turnSmooth); 
        }
    }
}
