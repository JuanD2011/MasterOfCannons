using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Character : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void CannonEnterReset(Cannon _cannonEntered)
    {
        transform.SetParent(_cannonEntered.transform, true);
        //Rigidbody.velocity = Vector3.zero;
        //transform.rotation = Quaternion.identity;
        SetKinematic(true);
    }

    public void SetKinematic(bool _value) => Rigidbody.isKinematic = _value;
}
