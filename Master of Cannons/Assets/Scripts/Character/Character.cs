using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        MenuGameManager.OnPause += Freeze;
        Referee.OnGameOver += Freeze;
    }

    private void Update()
    {
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
