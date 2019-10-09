using UnityEngine;

public class LoadingCircle : MonoBehaviour
{
    [SerializeField] Transform loadingCharge;
    [SerializeField] float chargeSpeed = 5f;

    void Update() => loadingCharge.Rotate(Vector3.back * chargeSpeed * 50 * Time.deltaTime);
}
