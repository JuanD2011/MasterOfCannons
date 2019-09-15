using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    protected float shootForce = 10f, wickLength = 5f;

    [SerializeField]
    private Vector3 catchRotation = new Vector3(0, 0, 180f);

    protected bool burningWick = false;
    protected float elapsedWickTime = 0f;
    protected Transform reference;
    private Character characterInCannon = null;

    private void Awake()
    {
        PlayerInputHandler.OnShootAction -= Shoot;
    }

    protected virtual void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Reference"))
            {
                reference = transform.GetChild(i);
            }
        }

        PlayerInputHandler.OnShootAction += Shoot;
    }

    protected virtual void Update()
    {
        if (burningWick) elapsedWickTime += Time.deltaTime;

        if (elapsedWickTime > wickLength) Shoot();
    }

    protected virtual void Shoot()
    {
        if (characterInCannon != null)
        {
            burningWick = false;
            characterInCannon.transform.SetParent(null);
            characterInCannon.SetKinematic(false);
            characterInCannon.Rigidbody.AddForce(transform.up * shootForce, ForceMode.Impulse);
            characterInCannon = null; 
        }
    }

    protected void CatchCharacter()
    {
        characterInCannon.transform.position = reference.position;
        characterInCannon.CannonEnterReset(reference);
        CatchRotation();
    }

    protected virtual void CatchRotation()
    {
        LeanTween.rotate(gameObject, catchRotation, 0.25f);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            characterInCannon = other.GetComponent<Character>();
            CatchCharacter();
        }
    }
}
