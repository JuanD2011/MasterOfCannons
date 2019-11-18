using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private float wickLength;

    [SerializeField]
    protected float shootForce = 10f;

    [SerializeField]
    bool doCatchRotation = false;

    [SerializeField]
    private Vector3 catchRotation = new Vector3(0, 0, 180f);

    protected bool burningWick = false;
    protected float elapsedWickTime = 0f;
    protected Transform reference = null;
    private Character characterInCannon = null;
    
    protected float WickLength { get => wickLength * GlobalMultipliers.WickLenght; private set => wickLength = value; }

    public event Delegates.Action<bool> OnCharacterInCannon = null;

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
        if (burningWick && !MenuGameManager.IsPaused) elapsedWickTime += Time.deltaTime;
        if (elapsedWickTime > WickLength) Shoot();
    }

    private void Shoot()
    {
        if (characterInCannon != null)
        {
            characterInCannon.UpdateSpecialProgress(elapsedWickTime / WickLength);
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
        burningWick = true;
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
