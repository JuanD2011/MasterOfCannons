using UnityEngine;

public class AimingBehaviour : MonoBehaviour
{
    private float targetRotation = 0f;

    private Vector3 direction = Vector3.zero;

    private Camera m_Camera = null;

    private bool canAim = false;

    private Cannon cannon = null;

    private void Awake()
    {
        cannon = GetComponentInChildren<Cannon>();
    }

    private void Start()
    {
        m_Camera = Camera.main;
        cannon.OnCharacterInCannon += SetCanAim;
    }

    private void SetCanAim(bool _value) => canAim = _value;

    private void Update()
    {
        if (canAim && !MenuGameManager.IsPaused)
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
}
