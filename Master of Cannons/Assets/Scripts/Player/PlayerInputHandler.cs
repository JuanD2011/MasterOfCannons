using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputHandler : MonoBehaviour, IPointerClickHandler
{
    public static event System.Action OnShootAction = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnShootAction?.Invoke();
    }
}
