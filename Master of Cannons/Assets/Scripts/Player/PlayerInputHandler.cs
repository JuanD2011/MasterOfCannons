using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputHandler : MonoBehaviour, IPointerClickHandler
{
    public static event Delegates.Action OnShootAction = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnShootAction?.Invoke();
    }
}
