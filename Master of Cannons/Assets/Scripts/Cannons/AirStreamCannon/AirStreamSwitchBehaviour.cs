using UnityEngine;

[RequireComponent(typeof(AirStreamBehaviour))]
public class AirStreamSwitchBehaviour : MonoBehaviour
{
    [SerializeField] private float timeToChange = 0f;

    public event Delegates.Action OnSwich = null;
}
