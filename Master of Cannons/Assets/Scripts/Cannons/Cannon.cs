using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private float wickLength = 5f;

    [SerializeField]
    protected float shootForce = 18f;

    [SerializeField]
    bool doCatchRotation = false;

    [SerializeField]
    private Vector3 catchRotation = new Vector3(0, 0, 180f);

    protected bool burningWick = false;
    protected float elapsedWickTime = 0f;
    private Character characterInCannon = null;
    
    protected float WickLength { get => wickLength * GlobalMultipliers.WickLenght; private set => wickLength = value; }
    public Transform Reference { get; protected set; } = null;

    public event Delegates.Action<bool> OnCharacterInCannon = null;
    public static event Delegates.Action<Vector3> OnChangeLosingBoundaries = null;

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
                Reference = transform.GetChild(i);
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
        OnChangeLosingBoundaries?.Invoke(transform.position);
        burningWick = true;
        characterInCannon.transform.position = Reference.position;
        characterInCannon.CannonEnterReset(Reference);

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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character")) characterInCannon = null;
    }
}
