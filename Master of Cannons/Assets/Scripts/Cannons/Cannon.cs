using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    protected float shootForce = 10f, wickLength = 5f;

    [SerializeField]
    bool doCatchRotation = false;

    [SerializeField]
    private Vector3 catchRotation = new Vector3(0, 0, 180f);

    protected bool burningWick = false;
    protected float elapsedWickTime = 0f;
    protected Transform reference = null;
    private Character characterInCannon = null;

    public event System.Action<bool> OnCharacterInCannon = null;

    protected virtual void Awake()
    {
        PlayerInputHandler.OnShootAction -= Shoot;
    }

    private void Start()
    {
        GetReference();

        PlayerInputHandler.OnShootAction += Shoot;
    }

    private void GetReference()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Reference"))
            {
                reference = transform.GetChild(i);
            }
        }
    }

    private void Update()
    {
        if (burningWick && !MenuManager.IsPaused) elapsedWickTime += Time.deltaTime;

        if (elapsedWickTime > wickLength) Shoot();
    }

    private void Shoot()
    {
        if (characterInCannon != null)
        {
            burningWick = false;
            elapsedWickTime = 0f;
            characterInCannon.transform.SetParent(null);
            characterInCannon.SetKinematic(false);
            characterInCannon.Rigidbody.AddForce(transform.up * shootForce, ForceMode.Impulse);
            characterInCannon = null;
            OnCharacterInCannon?.Invoke(false);
        }
    }

    private void CatchCharacter()
    {
        OnCharacterInCannon?.Invoke(true);
        characterInCannon.transform.position = reference.position;
        characterInCannon.CannonEnterReset(reference);

        if (doCatchRotation) CatchRotation();
    }

    private void CatchRotation()
    {
        LeanTween.rotate(gameObject, catchRotation, 0.25f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            characterInCannon = other.GetComponentInParent<Character>();
            CatchCharacter();
        }
    }
}
