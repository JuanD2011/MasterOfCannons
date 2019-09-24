using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class AirStreamBehaviour : MonoBehaviour
{
    [SerializeField]
    private AirStreamType type = AirStreamType.None;

    [SerializeField]
    private float streamMaxForce = 0f;

    private float range = 0f, distance = 0f;

    private bool playerInStreamZone = false;

    private Rigidbody playerRigidbody = null;

    private Cannon cannon = null;

    private void Awake()
    {
        range = GetComponent<BoxCollider>().size.y;
        cannon = GetComponentInChildren<Cannon>();
    }

    private void Start()
    {
        cannon.OnCharacterInCannon += OnCharacterInCannon;        
    }

    private void OnCharacterInCannon(bool _value)
    {
        if(_value)
        {
            playerInStreamZone = false;
            playerRigidbody = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInStreamZone = true;
        playerRigidbody = other.GetComponent<Rigidbody>();
    }

    private void OnTriggerExit(Collider other)
    {
        playerInStreamZone = false;
        playerRigidbody = null;
    }

    private void FixedUpdate()
    {
        if (playerInStreamZone && playerRigidbody != null)
        {
            AirStreamForce(); 
        }
    }

    private void AirStreamForce()
    {
        Vector3 direction = Vector3.zero;

        switch (type)
        {
            case AirStreamType.Suck:
                direction = -transform.up;
                break;
            case AirStreamType.Blow:
                direction = transform.up;
                break;
            case AirStreamType.None:
                break;
            default:
                break;
        }

        float distance = (transform.position - playerRigidbody.transform.position).sqrMagnitude;
        float force = distance / range * streamMaxForce;

        playerRigidbody.AddForce(direction * force, ForceMode.Force);
    }

    private void Reset()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }
}

public enum AirStreamType
{
    Suck,
    Blow,
    None
}
