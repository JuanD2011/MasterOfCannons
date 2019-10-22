using UnityEngine;

public abstract class Boss : MonoBehaviour
{
    [SerializeField]
    protected float life = 0f;

    [SerializeField]
    protected Character character = null;
}
