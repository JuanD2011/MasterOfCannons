using UnityEngine;

[RequireComponent(typeof(AirStreamBehaviour))]
public class AirStreamSwitchBehaviour : MonoBehaviour
{
    [SerializeField] private float timeToChange = 0f;

    public event Delegates.Action OnSwich = null;

    private float time = 0f;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > timeToChange)
        {
            time = 0f;
            OnSwich?.Invoke();
        }
    }
}
