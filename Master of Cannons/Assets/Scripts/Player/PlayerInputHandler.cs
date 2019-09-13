using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    public static event System.Action OnShootAction;

    public void OnShoot()
    {
        OnShootAction?.Invoke();
    }
}
