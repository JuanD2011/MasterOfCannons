using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    protected float shootForce = 10f, wickLength = 5f, CatchRotationAngle = 180f;

    [SerializeField]
    protected iTween.EaseType CatchRotationEaseType;

    protected bool burningWick = false;
    protected float elapsedWickTime = 0f;
    private Vector3 reference;
    private Character characterInCannon = null;

    protected internal string hashPosition = "position", hashEaseType = "easetype", hashIgnoreTimeScale = "ignoretimescale", hashRotation = "rotation", hashSpeed = "speed", hashOnComplete = "oncomplete", hashTime = "time";

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).CompareTag("Reference"))
            {
                reference = transform.GetChild(i).position;
            }
        }
    }

    protected virtual void Update()
    {
        if (burningWick) elapsedWickTime += Time.deltaTime;

        if (elapsedWickTime > wickLength) Shoot();
    }

    protected virtual void Shoot()
    {
        burningWick = false;
        characterInCannon.transform.SetParent(null);
        characterInCannon.Rigidbody.AddForce(transform.up * shootForce, ForceMode.Impulse);
        characterInCannon = null;
    }

    protected void CatchCharacter()
    {
        CatchRotation();
        characterInCannon.CannonEnterReset(this);
        characterInCannon.transform.position = reference;
    }

    protected virtual void CatchRotation()
    {
        iTween.RotateTo(gameObject, iTween.Hash(hashRotation, CatchRotationAngle, hashEaseType, CatchRotationEaseType, hashTime, 0.5f, hashIgnoreTimeScale, true));
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            characterInCannon = other.GetComponent<Character>();
            CatchCharacter();
        }
    }
}
