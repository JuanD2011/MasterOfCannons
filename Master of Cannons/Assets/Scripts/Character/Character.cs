using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{

    private float specialProgress = 0f;
    protected float specialTime = 4f;
    private bool canActivateSpecial = false;
    protected bool hasSpecial = false;

    public Rigidbody Rigidbody { get; private set; }
    public Vector3 velocityUpdated { get; private set; }
    public bool CanActivateSpecial { get=> canActivateSpecial; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        hasSpecial = false;
    }

    protected virtual void Start()
    {
        MenuGameManager.OnPause += Freeze;
        Referee.OnGameOver += Freeze;
    }

    private void Update()
    {
        velocityUpdated = Rigidbody.velocity;

        //if (Rigidbody.velocity.y < 0)
        //{
        //    Rigidbody.velocity += Physics.gravity * Time.deltaTime;
        //}
    }

    public void CannonEnterReset(Transform _reference)
    {
        transform.SetParent(_reference.transform);
        //Rigidbody.velocity = Vector3.zero;
        //transform.rotation = Quaternion.identity;
        transform.localRotation = Quaternion.identity;
        SetKinematic(true);
    }

    public void SetKinematic(bool _value) => Rigidbody.isKinematic = _value;

    private void Freeze()
    {
        Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
    private void Freeze(bool _Value)
    {
        if (_Value)
        {
            Rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        }
        else
        {
            Rigidbody.constraints = RigidbodyConstraints.None;
        }
    }
}
