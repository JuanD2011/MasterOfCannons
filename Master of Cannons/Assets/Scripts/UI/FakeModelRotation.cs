using UnityEngine;
using UnityEngine.EventSystems;

public class FakeModelRotation : MonoBehaviour, IDragHandler
{
    public static event Delegates.Action<float> OnRotate = null;

    public void OnDrag(PointerEventData _EventData)
    {
        OnRotate( _EventData.delta.x);
    }
}
