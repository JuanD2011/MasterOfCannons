using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public static event System.Action OnShootAction = null;
    public static event System.Action<Vector2> OnAimAction = null;

    public void OnShoot()
    {
        OnShootAction?.Invoke();
    }

    public void OnAim(InputValue _Context)
    {
        //OnAimAction?.Invoke(_Context.Get<Vector2>());
    }
}
