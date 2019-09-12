using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    protected float shootForce = 10f, wickLength = 5f;

    protected bool burningWick = false;
    protected float elapsedWickTime = 0f;

    protected virtual void Update()
    {
        if (burningWick) elapsedWickTime += Time.deltaTime;

        if (elapsedWickTime > wickLength) Shoot();
    }

    protected virtual void Shoot()
    {
        burningWick = false;
    }

    protected void CatchCharacter()
    {

    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character")) CatchCharacter();
    }
}
